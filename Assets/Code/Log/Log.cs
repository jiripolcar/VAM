using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using Animations;

namespace ConsoleLog
{
    public class LogRecord
    {
        static int logN = 0;
        int number;
        internal float[] times;

        public LogRecord(int steps)
        {
            number = logN++;
            times = new float[steps];
        }

        public override string ToString()
        {
            string s = "" + number;
            foreach (float f in times)
                s += ";" + f.ToString("0.000");
            return s;
        }
    }

    public class Log : MonoBehaviour
    {
        /// <summary>
        /// Instance of the Log MonoBehaviour
        /// </summary>
        private static Log Carrier { get; set; }
        private Queue<string> logsWriteToFileBatch = new Queue<string>();
        [Tooltip("Set batch size, in which the log will be saved to file.")] [SerializeField] private int logBatch = 20;
        [SerializeField] private AnimationHolder animationHolder;

        void Awake()
        {
            if (Carrier == null)
                Carrier = this;
            else
                WriteToLog("An instance of Log already exists with name: " + Carrier.gameObject.name);
            if (!animationHolder)
                animationHolder = GetComponentInChildren<AnimationHolder>();
        }

        private void WriteHeader()
        {
            List<string> names = animationHolder.GetStepsNames();
            string s = "#";
            foreach (string n in names)
                s += ";" + n;
            Write(s);
        }

        private void Start()
        {
            WriteHeader();
            currentLog = new LogRecord(animationHolder.GetStepCount());
        }

        private void OnApplicationQuit()
        {
            WriteToFile();
        }

        private void OnDestroy()
        {
            WriteToFile();
        }

        /// <summary>
        /// Writes the log entry according to the setting of the Log MonoBehaviour instance.
        /// </summary>
        public static void Write(string message)
        {
            if (Carrier)
                Carrier.WriteToLog(message);
        }

        private void WriteToLog(string message)
        {
            logsWriteToFileBatch.Enqueue(message);
            if (logsWriteToFileBatch.Count >= logBatch)
                WriteToFile();
        }

        private string _logPath = "";
        private string logPath
        {
            get
            {
                if (_logPath == "")
                    _logPath = Configuration.Data.logFolder + "\\" + System.DateTime.Now.ToFileTime() + "." + Configuration.Data.logExtension;
                return _logPath;
            }
        }

        private void WriteToFile()
        {
            StreamWriter sw = new StreamWriter(logPath, true);
            while (logsWriteToFileBatch.Count > 0)
            {
                string s = logsWriteToFileBatch.Dequeue();
                sw.WriteLine(s.ToString());
            }
            sw.Close();
        }

        private LogRecord currentLog;
        private int lastStep = -1;

        public static void AddTime(int step, float time)
        {
            if (Carrier)
            {
                if (step < Carrier.lastStep)
                    CommitNewLog();
                Carrier.lastStep = step;

                if (step >= 0)
                {
                    Carrier.currentLog.times[step] = time;
                }
            }
        }

        public static void CommitNewLog()
        {
            if (Carrier)
            {
                Write(Carrier.currentLog.ToString().Replace(".", Configuration.Data.decimalDelimiter));
                Carrier.currentLog = new LogRecord(Carrier.animationHolder.GetStepCount());
            }
        }

    }
}