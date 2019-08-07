using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Assess
    {
        public static string Simple()
        {
            string uuid = Uuid.generate();
            string courseId = "mycourse";
            string questionsApiActivityJson = Assess.questionsApiActivity(uuid, courseId);
            JsonObject questionsApiActivity = JsonObjectFactory.fromString(questionsApiActivityJson);

            string service = "assess";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("user_id", "$ANONYMIZED_USER_ID");
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();
            request.set("name", "Demo Activity (8 questions)");
            request.set("state", "initial");
            request.set("items", Assess.items(uuid));
            request.set("questionsApiActivity", questionsApiActivity);

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        private static JsonObject items(string uuid)
        {
            JsonObject items = new JsonObject(true);

            for (int i = 3; i <= 10; i++)
            {
                JsonObject responseIDs = new JsonObject(true);
                responseIDs.set(uuid + "_Demo" + i.ToString());
                JsonObject item = new JsonObject();
                item.set("reference", "Demo" + i.ToString());
                item.set("content", "<span class='learnosity-response question-" + uuid + "_Demo" + i.ToString() + "'></span>");
                item.set("workflow", new JsonObject(true));
                item.set("response_ids", responseIDs);
                item.set("feature_ids", new JsonObject(true));
                items.set(item);
            }

            return items;
        }

        private static string questionsApiActivity(string uuid, string courseId)
        {
            return string.Format(@"{{
                ""type"": ""submit_practice"",
                ""state"": ""initial"",
                ""id"": ""assessdemo"",
                ""name"": ""Assess API - Demo"",
                ""course_id"": ""{1}"",
                ""questions"": [
                 {{
                    ""type"": ""orderlist"",
                    ""list"": [
                      ""cat"",
                      ""horse"",
                      ""pig"",
                      ""elephant"",
                      ""mouse""
                    ],
                    ""stimulus"": ""<p>Arrange these animals from smallest to largest</p>"",
                    ""ui_style"": ""button"",
                    ""validation"": {{
                      ""show_partial_ui"": true,
                      ""partial_scoring"": true,
                      ""valid_score"": 1,
                      ""penalty_score"": 0,
                      ""valid_response"": [
                        4,
                        0,
                        2,
                        1,
                        3
                      ],
                      ""pairwise"": false
                    }},
                    ""instant_feedback"": true,
                    ""response_id"": ""{0}_Demo3"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo3"",
                      ""widget_reference"": ""Demo3""
                    }}
                  }},
                  {{
                    ""type"": ""clozeassociation"",
                    ""template"": ""<p> <strong>The United States of America was founded in {{{{response}}}}.</strong></p>"",
                    ""possible_responses"": [
                      ""1676"",
                      ""1776"",
                      ""1876""
                    ],
                    ""feedback_attempts"": 2,
                    ""instant_feedback"": true,
                    ""validation"": {{
                      ""show_partial_ui"": true,
                      ""partial_scoring"": true,
                      ""valid_score"": 1,
                      ""penalty_score"": 0,
                      ""valid_responses"": [
                        [
                          ""1776""
                        ]
                      ]
                    }},
                    ""response_id"": ""{0}_Demo4"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo4"",
                      ""widget_reference"": ""Demo4""
                    }}
                  }},
                  {{
                    ""type"": ""clozetext"",
                    ""template"": ""<p>What is the sum of \\\\(785 \\\\times 89\\\\)</p> {{{{response}}}}"",
                    ""is_math"": true,
                    ""validation"": {{
                      ""show_partial_ui"": true,
                      ""partial_scoring"": true,
                      ""valid_score"": 1,
                      ""penalty_score"": 0,
                      ""valid_responses"": [
                        [
                          ""69865""
                        ]
                      ]
                    }},
                    ""instant_feedback"": true,
                    ""response_id"": ""{0}_Demo5"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo5"",
                      ""widget_reference"": ""Demo5""
                    }}
                  }},
                  {{
                    ""type"": ""numberline"",
                    ""points"": [
                      ""5/5"",
                      ""1/4"",
                      ""2/4"",
                      ""7/8""
                    ],
                    ""is_math"": true,
                    ""labels"": {{
                      ""points"": ""0,1,2,3,4"",
                      ""show_min"": true,
                      ""show_max"": true
                    }},
                    ""line"": {{
                      ""min"": 0,
                      ""max"": 4,
                      ""left_arrow"": true,
                      ""right_arrow"": true
                    }},
                    ""stimulus"": ""<p>Drag the points onto the numberline.</p>"",
                    ""ticks"": {{
                      ""distance"": "".25"",
                      ""show"": true
                    }},
                    ""validation"": {{
                      ""partial_scoring"": ""true"",
                      ""show_partial_ui"": ""true"",
                      ""valid_score"": ""1"",
                      ""penalty_score"": ""0"",
                      ""threshold"": ""0"",
                      ""valid_responses"": [
                        {{
                          ""point"": ""5/5"",
                          ""position"": ""1""
                        }},
                        {{
                          ""point"": ""1/4"",
                          ""position"": "".25""
                        }},
                        {{
                          ""point"": ""2/4"",
                          ""position"": ""2""
                        }},
                        {{
                          ""point"": ""7/8"",
                          ""position"": ""3.5""
                        }}
                      ]
                    }},
                    ""instant_feedback"": true,
                    ""snap_to_ticks"": true,
                    ""response_id"": ""{0}_Demo6"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo6"",
                      ""widget_reference"": ""Demo6""
                    }}
                  }},
                  {{
                    ""type"": ""tokenhighlight"",
                    ""template"": ""<p>He was told not to laugh in class.</p>"",
                    ""tokenization"": ""word"",
                    ""validation"": {{
                      ""show_partial_ui"": true,
                      ""partial_scoring"": true,
                      ""valid_score"": 1,
                      ""penalty_score"": 0,
                      ""valid_responses"": [
                        5
                      ]
                    }},
                    ""stimulus"": ""<p>Highlight the verb in the sentence below.</p>"",
                    ""instant_feedback"": true,
                    ""response_id"": ""{0}_Demo7"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo7"",
                      ""widget_reference"": ""Demo7""
                    }}
                  }},
                  {{
                    ""type"": ""mcq"",
                    ""options"": [
                      {{
                        ""value"": ""0"",
                        ""label"": ""Berlin""
                      }},
                      {{
                        ""value"": ""1"",
                        ""label"": ""Paris""
                      }},
                      {{
                        ""value"": ""2"",
                        ""label"": ""London""
                      }},
                      {{
                        ""value"": ""3"",
                        ""label"": ""Madrid""
                      }}
                    ],
                    ""stimulus"": ""<strong>What\'s the capital of France?</strong>"",
                    ""stimulus_review"": ""Something Else"",
                    ""ui_style"": {{
                      ""type"": ""block"",
                      ""choice_label"": ""upper-alpha""
                    }},
                    ""valid_responses"": [
                      {{
                        ""value"": ""1"",
                        ""score"": 1
                      }}
                    ],
                    ""instant_feedback"": true,
                    ""response_id"": ""{0}_Demo8"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo8"",
                      ""widget_reference"": ""Demo8""
                    }}
                  }},
                  {{
                    ""type"": ""clozedropdown"",
                    ""template"": ""“It’s all clear,’ he {{{{response}}}}. “Have you the chisel and the bags? Great Scott! Jump, Archie, jump, and I’ll swing for it!’ Sherlock {{{{response}}}} had sprung out and seized the {{{{response}}}} by the collar. The other dived down the hole, and I heard the sound of {{{{response}}}} cloth as Jones clutched at his skirts. The light flashed upon the barrel of a revolver, but Holmes’ {{{{response}}}} came down on the man’s wrist, and the pistol {{{{response}}}} upon the stone floor."",
                    ""possible_responses"": [
                      [
                        ""whispered"",
                        ""sprinted"",
                        ""joked""
                      ],
                      [
                        ""Homes"",
                        ""holmes"",
                        ""Holmes""
                      ],
                      [
                        ""acquaintance"",
                        ""intruder"",
                        ""shopkeeper""
                      ],
                      [
                        ""burning"",
                        ""departing"",
                        ""rending"",
                        ""broken""
                      ],
                      [
                        ""revolver"",
                        ""hunting crop""
                      ],
                      [
                        ""rattled"",
                        ""clinked"",
                        ""spilt""
                      ]
                    ],
                    ""stimulus"": ""<p><strong>Fill in the blanks.</strong></p>"",
                    ""validation"": {{
                      ""show_partial_ui"": true,
                      ""partial_scoring"": true,
                      ""valid_score"": 1,
                      ""penalty_score"": 0,
                      ""valid_responses"": [
                        [
                          ""whispered""
                        ],
                        [
                          ""Holmes""
                        ],
                        [
                          ""intruder""
                        ],
                        [
                          ""rending""
                        ],
                        [
                          ""hunting crop""
                        ],
                        [
                          ""clinked""
                        ]
                      ]
                    }},
                    ""instant_feedback"": true,
                    ""response_id"": ""{0}_Demo9"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo9"",
                      ""widget_reference"": ""Demo9""
                    }}
                  }},
                  {{
                    ""type"": ""graphplotting"",
                    ""axis_x"": {{
                      ""ticks_distance"": 1,
                      ""draw_labels"": true
                    }},
                    ""axis_y"": {{
                      ""ticks_distance"": 1,
                      ""draw_labels"": true
                    }},
                    ""canvas"": {{
                      ""x_min"": 0,
                      ""x_max"": 10.2,
                      ""y_min"": -0.5,
                      ""y_max"": 10.2,
                      ""snap_to"": ""grid""
                    }},
                    ""grid"": {{
                      ""x_distance"": 1,
                      ""y_distance"": 1
                    }},
                    ""toolbar"": {{
                      ""tools"": [
                        ""point"",
                        ""move""
                      ],
                      ""default_tool"": ""point""
                    }},
                    ""ui_style"": {{
                      ""margin"": ""10px""
                    }},
                    ""stimulus"": ""<p>Plot the following points \\\\((2,5), (4,8), (8,1)\\\\).</p>"",
                    ""is_math"": true,
                    ""validation"": {{
                      ""valid_score"": ""1"",
                      ""penalty_score"": ""0"",
                      ""valid_responses"": [
                        [
                          {{
                            ""id"": ""lrn_1"",
                            ""type"": ""point"",
                            ""coords"": {{
                              ""x"": 2,
                              ""y"": 5
                            }}
                          }},
                          {{
                            ""id"": ""lrn_2"",
                            ""type"": ""point"",
                            ""coords"": {{
                              ""x"": 4,
                              ""y"": 8
                            }}
                          }},
                          {{
                            ""id"": ""lrn_3"",
                            ""type"": ""point"",
                            ""coords"": {{
                              ""x"": 8,
                              ""y"": 1
                            }}
                          }}
                        ]
                      ]
                    }},
                    ""instant_feedback"": true,
                    ""response_id"": ""{0}_Demo10"",
                    ""metadata"": {{
                      ""sheet_reference"": ""Demo10"",
                      ""widget_reference"": ""Demo10""
                    }}
                  }}
                ]
            }}", uuid, courseId);
        }
    }
}
