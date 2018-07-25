using System;
using System.IO;
using Model;
using Processor.Helpers;
using Processor.Parsers;
using Model.Exceptions;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SuperHero;

namespace Processor
{
    public class Processor
    {
        private readonly IOutputFormatter _outputFormatter;
        private readonly IInputReader _inputReaders;

        public Processor() { }
       public Processor(IOutputFormatter outputFormatter)
        {
            if (outputFormatter == null)
            {
                throw new ArgumentNullException("outputFormatter");    
            }

            _outputFormatter = outputFormatter;
            IInputReaderFactory inputReaderFactory = new InputReaderFactory();
            _inputReaders = inputReaderFactory.LoadInputReaders();
        }

        public List<string> Process(string location)
        {
            try
            {
                List<string> result = new List<string>();
                var rawInput1 = _inputReaders.ReadIntoList(location);
                var rawInput = rawInput1.Select(x => x.Replace("\r\a", "").Trim()).Where(x => x.Length > 0).ToList();
                rawInput.MergerLine();
                //Classifier
                var sectionExtractor = new SectionExtractor();
                var sections = sectionExtractor.ExtractFrom(rawInput);//******提取Flag,分辨模板格式

                #region 后期区分模板
                //List<int> typeList = new List<int>();
                //for (int i = 0; i < sections.Count(); i+=2)
                //{
                //    if (sections[i].Content.Count>0)
                //    {
                //        typeList.Add(sections[i].StringType + sections[i + 1].StringType);
                //    }
                //}
                //IList<Section> chinaList; ;
                //int typeTemplate = GetResumeTemplate(typeList);//区分模板格式。
                //if (typeTemplate == 1 & typeTemplate == 2)//111000 or 101010
                //{
                //    //0 or 1
                //    chinaList = sections.Where(x => x.StringType == 0).ToList();
                //}
                //if (typeTemplate==3)
                //{

                //}
                #endregion

                //Category identification
                //TODO:********Flag归类
                IResourceLoader resourceLoader = new CachedResourceLoader(new ResourceLoader());
                var resumeBuilder = new ResumeBuilder(resourceLoader, true);//*******处理+归类的Flag
                var resumeC = resumeBuilder.Build(sections.Where(x => x.StringType == 0).ToList());

                IResourceLoader resourceLoader1 = new CachedResourceLoader(new ResourceLoader());
                var resumeBuilder1 = new ResumeBuilder(resourceLoader, false);//*******处理归类的Flag
                var resumeE = resumeBuilder1.Build(sections.Where(x => x.StringType == 1).ToList());



                //result[0] = _outputFormatter.Format(resumeC);
                //result[1] = _outputFormatter.Format(resumeE);

                if (resumeC.PhoneNumbers != null && resumeC.FirstName != null && resumeC.EmailAddress != null)
                {
                   result.Add(_outputFormatter.Format(resumeC));
                }

                if (resumeE.PhoneNumbers != null && resumeE.FirstName != null && resumeE.EmailAddress != null)
                {
                  result.Add(_outputFormatter.Format(resumeE));
                }
                return result;
            }
            catch (IOException ex)
            {
                throw new ResumeParserException("There's a problem accessing the file, it might still being opened by other application", ex);
            }            
        }

        public int GetResumeTemplate(List<int> typeList)
        {
            int result = 0;
            decimal typeA = 0;//111000
            decimal typeB = 0;//101010
            decimal typeC = 0;//1011010
            List<int> errorlistA = new List<int>();
            List<int> errorlistB = new List<int>();
            List<int> errorlistC = new List<int>();
            //typeA
            for (int i = 0; i < typeList.Count(); i++)
            {
                bool a = typeList[i] == 2 || typeList[i] == 0;

                if (a & (i + 1 < typeList.Count()))
                {
                    if (!(typeList[i] == typeList[i + 1]))
                    {
                        errorlistA.Add(typeList[i]);
                        i++;
                    }
                }
                else
                {
                    if (typeList[i] != 2 && typeList[i] != 0)
                    {
                        errorlistA.Add(typeList[i]);
                    }
                }
            }
            typeA = ((decimal)errorlistA.Count() / typeList.Count());

            //typeB
            for (int i = 0; i < typeList.Count(); i++)
            {
                bool a = typeList[i] == 1 || typeList[i] == 0;

                if (a & (i + 1 < typeList.Count()))
                {
                    if (!(typeList[i] == typeList[i + 1]))
                    {
                        errorlistB.Add(typeList[i]);
                        i++;
                    }
                }
                else
                {
                    if (typeList[i] != 1 && typeList[i] != 0)
                    {
                        errorlistB.Add(typeList[i]);
                    }
                }
                typeB = ((decimal)errorlistB.Count() / typeList.Count());
            }
            //typeC
            for (int i = 0; i < typeList.Count(); i++)
            {
            
            }
            if (typeA < typeB && typeA < typeC)
            {
                result = 1;
            }
            if (typeB < typeA && typeB < typeC)
            {
                result = 2;
            }
            if (typeC < typeA && typeC < typeB)
            {
                result = 3;
            }
            return result;
        }
    }
}
