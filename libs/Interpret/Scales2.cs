using System;
using System.Runtime.InteropServices;
using DAO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Professor
{
    internal sealed class Scales_Proc
    {
        // ScalesWindow preload and manage procedures

        // Custom types

        // For dynamic ScalesWindow allocation
        public static TestType[] daTest;
        public static short numTestScale;
        public static ScaleType[] daScale;
        public static short numScale;

        public static void LoadScales()
        {
            short J;
            float sDelta;
            short I;
            Database DBControl;
            Recordset RSScale;

            DBControl = UpgradeSupport.DAODBEngine_definst.Workspaces[0].OpenDatabase(Setting.GetControlBase(), false,
                                                                                      true, Type.Missing);
            RSScale = DBControl.OpenRecordset("SELECT * FROM [(SCALES)] ORDER BY [T_S_NAME], [S_S_NAME]",
                                              RecordsetTypeEnum.dbOpenSnapshot, RecordsetOptionEnum.dbReadOnly,
                                              Type.Missing);

            RSScale.MoveLast(0);
            RSScale.MoveFirst();
            numScale = (short) RSScale.RecordCount;
            if (numScale > 0)
            {
                daScale = new ScaleType[numScale - 1 + 1];

                numTestScale = 1;
                daTest = new TestType[numTestScale - 1 + 1];
                daTest[numTestScale - 1].strName = Convert.ToString(RSScale.Fields["T_S_NAME"].Value);
                daTest[numTestScale - 1].scaleStart = 0;
                daScale[0].strName = Conversions.ToCharArrayRankOne(RSScale.Fields["S_S_NAME"].Value);
                if (Information.IsDBNull(RSScale.Fields["S_NAME"].Value))
                {
                    daScale[0].strComment = "";
                }
                else
                {
                    daScale[0].strComment = Convert.ToString(RSScale.Fields["S_NAME"].Value);
                }
                RSScale.MoveNext();

                for (I = 1; I <= numScale - 1; I++)
                {
                    if (Convert.ToString(RSScale.Fields["T_S_NAME"].Value) != daTest[numTestScale - 1].strName)
                    {
                        daTest[numTestScale - 1].scaleEnd = (short) (I - 1);

                        numTestScale++;
                        Array.Resize(ref daTest, numTestScale - 1 + 1);
                        daTest[numTestScale - 1].strName = Convert.ToString(RSScale.Fields["T_S_NAME"].Value);
                        daTest[numTestScale - 1].scaleStart = I;
                    }
                    daScale[I].Initialize();
                    daScale[I].strName = Conversions.ToCharArrayRankOne(RSScale.Fields["S_S_NAME"].Value);

                    if (Information.IsDBNull(RSScale.Fields["S_NAME"].Value))
                    {
                        daScale[I].strComment = "";
                    }
                    else
                    {
                        daScale[I].strComment = Convert.ToString(RSScale.Fields["S_NAME"].Value);
                    }
                    daScale[I].sAverageValue = Conversions.ToSingle(RSScale.Fields["AVR"].Value);
                    daScale[I].sMinValue = Conversions.ToSingle(RSScale.Fields["Min"].Value);
                    daScale[I].sMaxValue = Conversions.ToSingle(RSScale.Fields["Max"].Value);
                    sDelta = (daScale[I].sMaxValue - daScale[I].sMinValue)/4;
                    daScale[I].asAPoint[0] = daScale[I].sMinValue;
                    for (J = 1; J <= 4; J++)
                    {
                        daScale[I].asAPoint[J] = daScale[I].asAPoint[J - 1] + sDelta;
                    }
                    daScale[I].asIPoint[0] = Conversions.ToSingle(RSScale.Fields["I_POINT_0"].Value);
                    daScale[I].asIPoint[1] = Conversions.ToSingle(RSScale.Fields["I_POINT_1"].Value);
                    daScale[I].asIPoint[2] = Conversions.ToSingle(RSScale.Fields["I_POINT_2"].Value);
                    daScale[I].asIPoint[3] = Conversions.ToSingle(RSScale.Fields["I_POINT_3"].Value);
                    daScale[I].asIPoint[4] = Conversions.ToSingle(RSScale.Fields["I_POINT_4"].Value);

                    RSScale.MoveNext();
                }
                daTest[numTestScale - 1].scaleEnd = (short) (I - 1);
            }

            RSScale.Close();
            DBControl.Close();
        }

        public static void CalculateScales(short nTester)
        {
            short K;
            short nTTest;
            short J;
            if (Archive_Proc.daArchiveRecord[nTester].testStart == - 1)
            {
                return;
            }
            // Iterate on all tests for choosen Tester
            for (J = Archive_Proc.daArchiveRecord[nTester].testStart;
                 J <= Archive_Proc.daArchiveRecord[nTester].testEnd;
                 J++)
            {
                // If string length above one test length
                if (Strings.Len(Archive_Proc.daArchiveTest[J].strScales) >= 6)
                {
                    nTTest = daTFindTestNumber(Archive_Proc.daArchiveTest[J].strName);
                    if (nTTest != - 1)
                    {
                        Archive_Proc.daArchiveTest[J].countScale =
                            (short) (Strings.Len(Archive_Proc.daArchiveTest[J].strScales)/6);
                        Archive_Proc.daArchiveTest = new Archive_Proc.ArchiveTestType[J + 1];
                        Archive_Proc.daArchiveTest = new Archive_Proc.ArchiveTestType[J + 1];
                        for (K = 0; K <= Archive_Proc.daArchiveTest[J].countScale - 1; K++)
                        {
                            Archive_Proc.daArchiveTest[J].daScales[K] =
                                (float)
                                (Conversion.Val(Strings.Mid(Archive_Proc.daArchiveTest[J].strScales, 6*K + 1, 6)));
                            Archive_Proc.daArchiveTest[J].daStandScales[K] =
                                ToStandard(Archive_Proc.daArchiveTest[J].daScales[K],
                                           (short) (daTest[nTTest].scaleStart + K));
                        }
                    }
                }
            }
        }

        public static float ToStandard(float sValue, short nTScale)
        {
            float returnValue;
            short J;
            float sPart;
            short I;
            float sStandard;
            sStandard = 0;
            if (daScale[nTScale].sMinValue != daScale[nTScale].sMaxValue)
            {
                for (I = 0; I <= 4; I++)
                {
                    sPart = daScale[nTScale].asIPoint[I];
                    for (J = 0; J <= 4; J++)
                    {
                        if (J != I && (daScale[nTScale].asAPoint[I] - daScale[nTScale].asAPoint[J]) != 0)
                        {
                            sPart = sPart*(sValue - daScale[nTScale].asAPoint[J])/
                                    (daScale[nTScale].asAPoint[I] - daScale[nTScale].asAPoint[J]);
                        }
                    }
                    sStandard = sStandard + sPart;
                }
            }
            else
            {
                //        DebugPrint "(ToStandard) Test data error"
            }
            returnValue = sStandard;
            return returnValue;
        }

        public static short daTFindScaleNumber(ref string strTestName, ref string strScaleName)
        {
            short returnValue;
            short J;
            short I;
            short nScaleNumber;

            nScaleNumber = - 1;

            for (I = 0; I <= numTestScale - 1; I++)
            {
                if (daTest[I].strName == strTestName)
                {
                    for (J = daTest[I].scaleStart; J <= daTest[I].scaleEnd; J++)
                    {
                        if (new string(daScale[J].strName) == strScaleName)
                        {
                            nScaleNumber = (short) (J - daTest[I].scaleStart);
                            break;
                        }
                    }
                }
            }
            if (I == numTestScale)
            {
                Debug_Support.DebugIPrint(strTestName + " Отсутствует тест таблице шкал ");
            }
            else if (nScaleNumber == - 1)
            {
                Debug_Support.DebugIPrint(strTestName + " Отсутствует шкала в таблице шкал " + strScaleName);
            }

            returnValue = nScaleNumber;
            return returnValue;
        }

        public static string dbFindAnswerOnQuestion(ref string strTestName, short nQuest, short nAns)
        {
            string returnValue;
            string strAnsText;
            Recordset RSAnswer = null;

            strAnsText = "";
            //RSAnswer = Interpretator.DBTest.OpenRecordset(strTestName + "_ANS", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);
            RSAnswer.FindFirst("([ANS_Q_N] = " + nQuest + ") And ([ANS_A_N] = " + nAns + ")");
            if (RSAnswer.NoMatch == false)
            {
                strAnsText = Convert.ToString(RSAnswer.Fields["ANS_A_TEXT"].Value);
            }
            else
            {
                Debug_Support.DebugIPrint(strTestName + " Отсутствует ответ " + nAns + " на вопрос " + nQuest);
            }

            returnValue = strAnsText;
            return returnValue;
        }

        public static short daTFindTestNumber(string strTestName)
        {
            short returnValue;
            short I;
            short nTestNumber;
            nTestNumber = - 1;
            for (I = 0; I <= numTestScale - 1; I++)
            {
                if (daTest[I].strName == strTestName)
                {
                    nTestNumber = I;
                    break;
                }
            }
            if (I == numTestScale)
            {
                //        DebugIPrint strTestName + " Отсутствует тест таблице шкал "
            }

            returnValue = nTestNumber;
            return returnValue;
        }

        public static void daATestToText(short nTestNumber, ref string strText)
        {
            string strStandardValue;
            float sStandardValue;
            string strValue;
            string strComment;
            string strScale;
            short I;
            short nTTest;
            strText = "SCIENTEX GRAPH DATA" + '\r' + '\n' + "SCALES" + '\r' + '\n';
            strText = strText + "Title: " + daArchiveTest[nTestNumber].strName + '\r' + '\n';
            nTTest = Scales_Proc.daTFindTestNumber(daArchiveTest[nTestNumber].strName.ToString());
            for (I = 0; I <= daArchiveTest[nTestNumber].countScale - 1; I++)
            {
                strScale = Scales_Proc.daScale[Scales_Proc.daTest[nTTest].scaleStart + I].strName.ToString();
                strComment = '\u0022' + Scales_Proc.daScale[Scales_Proc.daTest[nTTest].scaleStart + I].strComment + '\u0022';
                strValue = Strings.Mid(daArchiveTest[nTestNumber].strScales, 6 * I + 1, 6);
                sStandardValue = daArchiveTest[nTestNumber].daStandScales[I];
                if (sStandardValue > 10)
                {
                    sStandardValue = sStandardValue / 10;
                    if (sStandardValue > 10)
                    {
                        sStandardValue = 0;
                    }
                }
                strStandardValue = Conversion.Str(sStandardValue);
                strText = strText + "Scale: " + strScale + " " + strComment + " " + strValue + " " + strStandardValue + '\r' + '\n';
            }
        }

        #region Nested type: MinMaxType

        public struct MinMaxType
        {
            public short nMax;
            public short nMin;
        }

        #endregion

        #region Nested type: ScaleType

        public struct ScaleType
        {
            //UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
            [VBFixedArray(4)] public float[] asAPoint;
            [VBFixedArray(4)] public float[] asIPoint;
            public float sAverageValue;
            public float sMaxValue;
            public float sMinValue;
            public string strComment;
            [VBFixedString(2), MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public char[] strName;

            //UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
            public void Initialize()
            {
                asAPoint = new float[5];
                asIPoint = new float[5];
            }
        }

        #endregion

        #region Nested type: TestType

        public struct TestType
        {
            public short scaleEnd;
            public short scaleStart;
            public string strName;
        }

        #endregion
    }
}