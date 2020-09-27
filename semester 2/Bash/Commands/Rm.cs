﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands
{
    public class Rm : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Rm(List<string> arguments, bool isFirst)
        {
            Arguments = ArgumentsConverter(arguments);
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        private List<string> ArgumentsConverter(List<string> input)
        {
            List<string> newArguments = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < input.Count; i++)
            {
                stringBuilder.Insert(stringBuilder.Length, input[i]);

                if (i < input.Count - 1 && input[i + 1].Contains(@":\") == true)
                {
                    newArguments.Add(stringBuilder.ToString());
                    stringBuilder.Remove(0, stringBuilder.Length);
                    continue;
                }

                if (i != input.Count - 1)
                {
                    stringBuilder = stringBuilder.Insert(stringBuilder.Length, " ");
                }

                if (i == input.Count - 1)
                {
                    newArguments.Add(stringBuilder.ToString());
                }
            }

            return (newArguments);
        }

        public void Execute()
        {
            for (int i = 0; i < Arguments.Count; i++)
            {
                try
                {
                    System.IO.File.Delete(Arguments[i]); //exist
                    Result.Add(Arguments[i]);
                }
                catch (ArgumentException e)
                {
                    throw e;
                }

            }

        }

        public bool IsCorrectArgs()
        {
            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new Exception("Incorrect rm command args");
            }

            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect rm command args");
            }
            return true;
        }
    }
}