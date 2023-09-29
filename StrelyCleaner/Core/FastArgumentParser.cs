using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;


// ***********************************************************************
// Author   : Destroyer
// Github   : https://github.com/DestroyerDarkNess
// Modified : 26-1-2023
// ***********************************************************************
// <copyright file="FastArgumentParser.cs" company="S4lsalsoft">
//     Copyright (c) S4lsalsoft. All rights reserved.
// </copyright>
// ***********************************************************************

namespace StrelyCleaner.Core
{
    public class FastArgumentParser
    {
        private List<IArgument> ArgumentList { get; set; }
        public string ArgumentDelimiter { get; set; } = "-";

        private List<IArgument> UnknownArgs = new List<IArgument>();
        public List<IArgument> UnknownArguments
        {
            get
            {
                return UnknownArgs;
            }
        }

        public int Count
        {
            get
            {
                return ArgumentList.Count();
            }
        }

        public FastArgumentParser()
        {
            ArgumentList = new List<IArgument>();
        }

        public IArgument Add(string name)
        {
            if (name.StartsWith(ArgumentDelimiter) == false)
                name = ArgumentDelimiter + name;
            IArgument ArgHandler = new IArgument() { Name = name };
            ArgumentList.Add(ArgHandler);
            return ArgHandler;
        }

        public void Parse(string[] args, string ParameterDelimiter = " ")
        {
            StringCollection argCol = new StringCollection();
            argCol.AddRange(args);

            StringEnumerator strEnum = argCol.GetEnumerator();

            IArgument LastArg = null;

            while (strEnum.MoveNext())
            {
                if (strEnum.Current.StartsWith(ArgumentDelimiter))
                {
                    IArgument GetArg = GetArgCommand(strEnum.Current);
                    LastArg = GetArg;

                    if (GetArg == null)
                    {
                        IArgument UnknownA = new IArgument() { Name = strEnum.Current };
                        UnknownArgs.Add(UnknownA);
                    }
                }
                else if (LastArg != null)
                {
                    if (string.IsNullOrWhiteSpace(LastArg.Value) == false)
                        LastArg.Value += ParameterDelimiter;
                    LastArg.Value += strEnum.Current;
                    continue;
                }
            }
        }

        private IArgument GetArgCommand(string NameEx)
        {
            foreach (var item in ArgumentList)
            {
                if (NameEx.Equals(item.Name))
                    return item;
            }
            return null;
        }
    }

    public class IArgument
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public IArgument SetDescription(string _text)
        {
            this.Description = _text;
            return this;
        }
    }
}

