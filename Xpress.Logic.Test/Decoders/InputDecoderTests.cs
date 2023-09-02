using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Xpress.Logic.Decoders;

namespace Xpress.Logic.Test
{
    [TestClass]
    public class InputDecoderTests
    {
        private InputDecoder _inputDecoder;

        private List<Language> languages = new List<Language>()
        { new Language(){Name = "EN" }, new Language(){Name = "NO" }, new Language(){Name = "SV" } };

        private List<string> cases = new List<string>()
        {
            #region case1
            "EN: hello. welome!\n" +
            "NO: Welcome:\n" +
            "SV: Wilkommen:\n" +
            "\n" +
            "EN: some test\n" +
            "NO: some other test\n" +
            "\n" +
            "EN: some ttest",
            #endregion
            #region case2
            "EN: hello. welome!\n" +
            "NO: Welcome:\n" +
            "SV: Wilkommen:\n" +
            "EN: some test\n" +
            "NO: some other test\n" +
            "EN: some ttest",
            #endregion
            #region case3
            "\n" +
            "\n" +
            "EN: hello. welome!\n" +
            "NO: Welcome:\n" +
            "SV: Wilkommen:\n" +
            "\n" +
            "\n" +
            "\n" +
            "EN: some test\n" +
            "NO: some other test\n" +
            "\n" +
            "\n" +
            "EN: some ttest" +
            "\n" +
            "\n"
            #endregion
        };

        private List<UserRequest> expected;
        
        [TestInitialize]
        public void Initialize() {
            _inputDecoder = new InputDecoder(new List<string>() { "en", "no", "sv" });
            expected = new List<UserRequest>()
            {
                #region case123
                new UserRequest()
                {
                    Languages = languages,
                    Records = new List<Record>()
                    {
                        new Record()
                        {
                           Key = null,
                           Values = new List<LocalizedRecord>()
                           {
                               new LocalizedRecord()
                               {
                                   Language = languages[0],
                                   Value = "hello. welome!"
                               },
                               new LocalizedRecord()
                               {
                                   Language = languages[1],
                                   Value = "Welcome:"
                               },
                               new LocalizedRecord()
                               {
                                   Language = languages[2],
                                   Value = "Wilkommen:"
                               },
                           }
                        },
                        new Record()
                        {
                           Key = null,
                           Values = new List<LocalizedRecord>()
                           {
                               new LocalizedRecord()
                               {
                                   Language = languages[0],
                                   Value = "some test"
                               },
                               new LocalizedRecord()
                               {
                                   Language = languages[1],
                                   Value = "some other test"
                               },
                           }
                        },
                         new Record()
                        {
                           Key = null,
                           Values = new List<LocalizedRecord>()
                           {
                               new LocalizedRecord()
                               {
                                   Language = languages[0],
                                   Value = "some ttest"
                               },
                           }
                        }
                    }
                }
                #endregion
            };

        }


        [TestMethod]
        public void Case1_ShouldDecodeProperly_CommonCase()
        {
            expected[0].Should().BeEquivalentTo(_inputDecoder.Decode(cases[0]));
        }

        [TestMethod]
        public void Case2_ShouldDecodeProperly_NoSpaces()
        {
            expected[0].Should().BeEquivalentTo(_inputDecoder.Decode(cases[1]));
        }

        [TestMethod]
        public void Case3_ShouldDecodeProperly_WithManySpaces()
        {
            expected[0].Should().BeEquivalentTo(_inputDecoder.Decode(cases[2]));
        }
    }
}
