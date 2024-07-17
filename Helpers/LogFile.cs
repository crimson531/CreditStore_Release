using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.Plugin;
using ChronoArkMod.Template;
using Debug = UnityEngine.Debug;
using ChronoArkMod.ModData;
using HarmonyLib;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace CreditStore_Release
{
    public static class LogFile
    {
        private static string _desktop;
        private const string _defaultFolder = "Chrono Ark Log Files";
        //private const string _defaultLogName = "Chrono Ark General Log.txt";
        public static bool Append = true;
        public static bool AddNewLine = true;
        private static bool _newFile = false;
        public static bool NewFile
        {
            get
            {
                return _newFile;
            }
            set
            {
                _newFile = value;
            }
        }
        /// <summary>
        /// Used to write text to file based on Append Variable. If append add text to file, otherwise overwrite if it exists.
        /// Param 1 -> Full File Name
        /// Param 2 -> Contents to write
        /// </summary>
        private static Action<string, string> Log
        {
            get
            {
                if (Append)
                    return new Action<string, string>((path, text) => { File.AppendAllText(path, AppendNewLine(text)); });
                else
                    return new Action<string, string>((path, text) => { File.WriteAllText(path, AppendNewLine(text)); });
            }
        }
        private static string AppendNewLine(string text) => AddNewLine ? text.AppendText("\r\n") : text;
        private static string WriteDir
        {
            get
            {
                if (_desktop == null) _desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                return Path.Combine(_desktop, _defaultFolder);
            }
        }
        private static string ToFilePath(string fileName) => 
            Path.Combine(WriteDir, FileNameAddTxtExt(fileName));
        static LogFile()
        {
            try
            {
                Init();

            }
            catch (Exception e)
            {
                Debug.Log("Log File Exception - " + e.ToString());
            }
        }
        static void Init()
        {
            CreateWriteDir();
        }
        static void CreateWriteDir()
        {
            if (!Directory.Exists(WriteDir))
            {
                Directory.CreateDirectory(WriteDir);
            }
        }
        public static void LogTextToDir(string text, string dirName, string fileName)
        {
            var newPath = Path.Combine(WriteDir, dirName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            newPath = Path.Combine(newPath, FileNameAddTxtExt(fileName));
            _LogText(text, newPath);
        }
        public static void LogTextDefault(string text)
        {
            _LogText(text, ToFilePath(_defaultFolder));
        }
        public static void LogText(string text, string fileName)
        {
            _LogText(text, ToFilePath(fileName));
        }
        private static void _LogText(string text, string fullName)
        {
            Log(fullName, text);
        }
        public static void MakeNewFile(string fileName)
        {
            File.WriteAllText(fileName, string.Empty);
        }
        public  static void MakeNewFile()
        {
            MakeNewFile(ToFilePath(_defaultFolder));
        }
        private static string FileNameAddTxtExt(string fileName)
        {
            if (!Path.HasExtension(fileName))
            {
                return $"{fileName}.txt";
            }
            else
            {
                var curExt = Path.GetExtension(fileName);
                return fileName.Replace(curExt, ".txt");
            }
        }
    }

}
