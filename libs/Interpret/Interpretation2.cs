using System.Diagnostics;
using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Text;
using Microsoft.VisualBasic;

using System.Data;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.VisualBasic.Compatibility.VB6;



namespace Interpret
{
    sealed class Interpretator
    {
        public static DAO.Database DBTest;
        public static bool bInterpretateBreakFlag;

        // For debug
        public static short nRuleNum;
        public static short nRuleCode;

        //
        static short numTester;
        static string strTestSelected;

        static short nGoToRuleNumber;
        static string strRuleText;
        static bool bRuleText;
        static short nInsertTestNumber;
        static short nRange;

        static short numSDScales;
        static short[] anScale = new short[5];

        static public void SemanticDiff(short nScale1, short nScale2, ref string strTest)
        {
            short nTTest;
            int nextQuest;
            short nCoeffCount;
            float sCoeff;
            short numAns;
            int numQuest;
            string bmQuest;
            short nQuestInTable;
            short numQuestion;
            short I;
            short nTest;
            DAO.Database DBTestLoc;
            DAO.Recordset RSQuest;
            DAO.Recordset RSAns;
            DAO.Recordset RSCoeff;
            DAO.Recordset RSCoeffOnAns;
            DAO.Recordset RSAnsOnQuest;

            nTest = Archive_Proc.daAFindTestNumber(numTester, strTest);
            if (nTest != -1)
            {
                // Throught all questions (like in TestWizard)
                float[] daScales = new float[Archive_Proc.daArchiveTest[nTest].countScale - 1 + 1];
                float[] daStandScales = new float[Archive_Proc.daArchiveTest[nTest].countScale - 1 + 1];
                for (I = 0; I <= Archive_Proc.daArchiveTest[nTest].countScale - 1; I++)
                {
                    daScales[I] = 0;
                }

                // Open Recordsets
                DBTestLoc = UpgradeSupport.DAODBEngine_definst.Workspaces[0].OpenDatabase(Setting.GetTestBase(), false, true, Type.Missing);
                RSQuest = DBTestLoc.OpenRecordset(strTest + "_QST", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);
                RSAns = DBTestLoc.OpenRecordset(strTest + "_ANS", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);
                RSCoeff = DBTestLoc.OpenRecordset(strTest + "_CF", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);

                numQuestion = 1;
                RSQuest.MoveLast(0);
                nQuestInTable = (short)RSQuest.RecordCount;
                // Find first question
                RSQuest.MoveFirst();
                bmQuest = Encoding.Unicode.GetString((byte[])RSQuest.get_Bookmark());
                RSQuest.FindFirst("QST_Q_N = 1");
                if (RSQuest.NoMatch)
                {
                    Array arrQuest = Encoding.Unicode.GetBytes(bmQuest);
                    RSQuest.set_Bookmark(ref arrQuest);
                }

                while (numQuestion <= nQuestInTable)
                {
                    // Proccess current question
                    numQuest = (int)RSQuest.Fields["QST_Q_N"].Value;
                    if ((int)RSQuest.Fields["QST_FLAG"].Value != 0 && (int)RSQuest.Fields["QST_FLAG"].Value != 2)
                    {
                        Debug_Support.DebugIPrint("(SemanticDiff) Error question FLAG");
                    }
                    RSAns.Filter = "[ANS_Q_N] = " + numQuest.ToString();
                    RSAns.Sort = "ANS_A_N";
                    RSAnsOnQuest = RSAns.OpenRecordset(DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly);
                    RSAns.Sort = "";
                    RSAns.Filter = "";

                    RSAnsOnQuest.MoveLast(0);
                    RSAnsOnQuest.MoveFirst();
                    numAns = (short)RSAnsOnQuest.RecordCount;

                    // Analyze scales here
                    sCoeff = System.Math.Abs(Strings.Asc(Strings.Mid(Archive_Proc.daArchiveTest[nTest].strAnswer, numQuestion + nQuestInTable * nScale1, 1)) - Strings.Asc(Strings.Mid(Archive_Proc.daArchiveTest[nTest].strAnswer, numQuestion + nQuestInTable * nScale2, 1))) / (numAns - 1);
                    RSCoeff.Filter = "[CF_Q_N]=" + numQuestion.ToString();
                    RSCoeff.Sort = "[CF_A_N]";
                    RSCoeffOnAns = RSCoeff.OpenRecordset(DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly);

                    RSCoeffOnAns.MoveLast(0);
                    RSCoeffOnAns.MoveFirst();
                    nCoeffCount = (short)RSCoeffOnAns.RecordCount;
                    for (I = 0; I <= Archive_Proc.daArchiveTest[nTest].countScale - 1; I++)
                    {
                        if (Strings.Asc(Strings.Mid(Archive_Proc.daArchiveTest[nTest].strAnswer, numQuestion + nQuestInTable * I, 1)) - 34 > nCoeffCount)
                        {
                            Debug_Support.DebugIPrint("(SemanticDiff) Information about Tester not right");
                        }
                        else
                        {
                            RSCoeffOnAns.FindFirst("CF_A_N=" + System.Convert.ToString(Strings.Asc(Strings.Mid(Archive_Proc.daArchiveTest[nTest].strAnswer, numQuestion + nQuestInTable * I, 1)) - 33));
                            if (!RSCoeffOnAns.NoMatch)
                            {
                                daScales[I] = daScales[I] + (float)RSCoeffOnAns.Fields["CF_COEF"].Value * sCoeff;
                            }
                        }
                    }

                    RSCoeff.Filter = "";
                    RSCoeff.Sort = "";
                    RSCoeffOnAns.Close();

                    // Find Next question for answer
                    RSAnsOnQuest.AbsolutePosition = (Strings.Asc(Strings.Mid(Archive_Proc.daArchiveTest[nTest].strAnswer, numQuestion + 1, 1)) - 33) - 1;
                    nextQuest = (int)RSAnsOnQuest.Fields["ANS_N_OF_N"].Value;
                    // Find Next question for question
                    if (nextQuest == 0)
                    {
                        nextQuest = (int)RSQuest.Fields["QST_N_OF_N"].Value;
                    }
                    // Move to Next
                    if (nextQuest != 0)
                    {
                        RSQuest.FindFirst("QST_Q_N = " + nextQuest.ToString());
                        if (RSQuest.NoMatch)
                        {
                            // Error
                            RSQuest.MoveNext();
                        }
                        numQuestion++;
                    }
                    else
                    {
                        RSQuest.MoveNext();
                        numQuestion++;
                    }

                }
                // Recalculate to standard
                nTTest = Scales_Proc.daTFindTestNumber(strTest);
                if (nTTest > 0)
                {
                    for (I = 0; I <= Archive_Proc.daArchiveTest[nTest].countScale - 1; I++)
                    {
                        daStandScales[I] = Scales_Proc.ToStandard(daScales[I], (short)(Scales_Proc.daTest[nTTest].scaleStart + I));
                    }
                }
                else
                {
                    Debug_Support.DebugIPrint("(SemanticDiff) Test not found");
                }
            }
            else
            {
                Debug_Support.DebugIPrint("(SemanticDiff) Test not found for Tester");
            }

            // TO DO: Do something with daScales(), daStandScales()

        }

        public static void InterpretateRules(ref string strInterpret, short nTester, Progress refProgress, Data refData)
        {
            MsgBoxResult res;
            int numScaleSD = 0;
            string strText = null;
            int nGoToRuleNum = 0;
            string varBookmark;
            short nBufferCode = 0;
            short nBufferNumber = 0;
            short nRet;
            string strRule;
            int nRulesFound;
            int nRulesCount;

            try
            {
                numTester = nTester;
                bInterpretateBreakFlag = false;

                bool bCommandEnd;
                string strBufferText = "";
                bool bBufferText;
                bool bBufferNumber;

                bool bData;
                bool bProgress;

                bData = !(refData == null || (refData == null));
                bProgress = !(refProgress == null || (refProgress == null));

                Scales_Proc.LoadScales();
                Scales_Proc.CalculateScales(numTester);

                // TO DO: Test code here
                if (strInterpret == "TEST")
                {
                    string questTest = "SD4";
                    SemanticDiff(0, 1, ref questTest);
                    return;
                }

                // Get rules
                DAO.Database DBInterpr = null;
                DAO.Recordset RSRules = null;
                DAO.Recordset RSText = null;

                DBTest = UpgradeSupport.DAODBEngine_definst.Workspaces[0].OpenDatabase(Setting.GetTestBase(), false, true, Type.Missing);
                if (strInterpret.Substring(0, 1) == "{")
                {
                    RSRules = DBTest.OpenRecordset(strInterpret, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);
                    if (bData)
                    {
                        RSText = DBTest.OpenRecordset(strInterpret + "_T", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);
                    }
                }
                else if (strInterpret.Substring(0, 1) == "_")
                {
                    DBInterpr = UpgradeSupport.DAODBEngine_definst.Workspaces[0].OpenDatabase(Setting.GetInterpretBase(), false, true, Type.Missing);
                    RSRules = DBInterpr.OpenRecordset(strInterpret, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);
                    if (bData)
                    {
                        RSText = DBInterpr.OpenRecordset(strInterpret + "_T", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbReadOnly, Type.Missing);
                    }
                }

                RSRules.MoveLast(0);
                RSRules.MoveFirst();
                nRulesCount = RSRules.RecordCount;
                nRulesFound = 0;

                if (bProgress)
                {
                    refProgress.ProgressBar1.Maximum = nRulesCount;
                    //refProgress.Label1.Caption = "Обрабатываются правила:"
                    refProgress.Label2.Text = "Осталось: ";
                    refProgress.Label3.Text = "Найдено:  ";
                    refProgress.Label4.Text = nRulesCount.ToString();
                    refProgress.Label5.Text = nRulesFound.ToString();
                }

                Debug_Support.DebugCreateLogFile(strInterpret);

                bCommandEnd = false;
                bRuleText = false;
                bBufferText = false;
                bBufferNumber = false;
                while (!(RSRules.EOF && bCommandEnd == false))
                {
                    // Compile rule
                    strRule = RSRules.Fields["CON_C_TEXT"].Value.ToString();
                    nRuleNum = (short)Convert.ToInt32(RSRules.Fields["CON_TXT_N"].Value);

                    if (bInterpretateBreakFlag)
                    {
                        break;
                    }

                    nRet = 0;
                    if (Strings.Asc(strRule.Substring(0, 1)) == 45)
                    {
                        nRet = CompileRule(ref strRule, 2);
                    }

                    if (nRet == 1)
                    {
                        nRulesFound++;
                        if (bRuleText)
                        {
                            bRuleText = false;
                            if (bBufferText)
                            {
                                bBufferText = false;
                                if (bData)
                                {
                                    SetText(strBufferText + "\r" + "\n", ref refData.RichTextBox1);
                                }
                            }
                            strBufferText = strRuleText;
                            bBufferText = true;
                        }
                        if (bBufferNumber)
                        {
                            //                bBufferNumber = False
                            // Find Rule text
                            if (bData)
                            {
                                RSText.FindFirst("[TXT_TXT_N] = " + nBufferNumber.ToString());
                                if (RSText.NoMatch == false && RSText.Fields["TXT_TXT_S"].Value.ToString() != "")
                                {
                                    if (Debug_Support.IsDebug())
                                    {
                                        refData.RichTextBox1.SelectedText = Strings.Chr(149) + Strings.Chr(174) + nBufferNumber.ToString() + Strings.Chr(169) + nBufferCode.ToString() + RSText.Fields["TXT_TXT_S"].Value + "\r\n";
                                        refData.RichTextBox1.SelectionStart = refData.RichTextBox1.SelectionStart + refData.RichTextBox1.SelectionLength;
                                    }
                                    else
                                    {
                                        SetText(RSText.Fields["TXT_TXT_S"].Value + "\r" + "\n", ref refData.RichTextBox1);
                                    }
                                    //                       Debug.Print "Rule = " + CStr(nBufferNumber%), nBufferCode%, "  " + Mid(RSText![TXT_TXT_S], 1, 20)
                                }
                            }
                        }
                        bBufferNumber = true;
                        nBufferNumber = nRuleNum;
                        nBufferCode = nRuleCode;
                    }

                    if (nRet == 4)
                    {
                        varBookmark = Encoding.Unicode.GetString((byte[])RSRules.get_Bookmark());
                        RSRules.FindFirst("[CON_TXT_N] = " + nGoToRuleNum.ToString());
                        if (RSRules.NoMatch)
                        {
                            Array arr = Encoding.Unicode.GetBytes(varBookmark);
                            RSRules.set_Bookmark(ref arr);
                            Debug_Support.DebugIPrint(" Неверный переход " + nGoToRuleNum.ToString());
                        }
                    }
                    else
                    {
                        RSRules.MoveNext();
                    }

                    if (nRet == 5)
                    {
                        bBufferText = false;
                    }

                    if (nRet == 6)
                    {
                        bCommandEnd = true;
                    }

                    if (nRet == 7)
                    {
                        if (bData)
                        {
                            if (bBufferNumber)
                            {
                                //                bBufferNumber = False
                                // Find Rule text
                                if (bData)
                                {
                                    RSText.FindFirst("[TXT_TXT_N] = " + nBufferNumber.ToString());
                                    if (RSText.NoMatch == false && RSText.Fields["TXT_TXT_S"].Value.ToString() != "")
                                    {
                                        if (Debug_Support.IsDebug())
                                        {
                                            refData.RichTextBox1.SelectedText = Strings.Chr(149) + Strings.Chr(174) + nBufferNumber.ToString() + Strings.Chr(169) + nBufferCode.ToString() + RSText.Fields["TXT_TXT_S"].Value + "\r" + "\n";
                                            refData.RichTextBox1.SelectionStart = refData.RichTextBox1.SelectionStart + refData.RichTextBox1.SelectionLength;
                                        }
                                        else
                                        {
                                            SetText(RSText.Fields["TXT_TXT_S"].Value + "\r" + "\n", ref refData.RichTextBox1);
                                        }
                                        //                       Debug.Print "Rule = " + CStr(nBufferNumber%), nBufferCode%, "  " + Mid(RSText![TXT_TXT_S], 1, 20)
                                    }
                                }
                            }
                            bBufferNumber = true;
                            nBufferNumber = nRuleNum;
                            nBufferCode = nRuleCode;

                            (new Microsoft.VisualBasic.Devices.Computer()).Clipboard.Clear();
                            Archive_Proc.daATestToText(nInsertTestNumber, ref strText);
                            (new Microsoft.VisualBasic.Devices.Computer()).Clipboard.SetText(strText);
                            // TO DO: add call the paste verb
                            //refData.RichTextBox1.OLEObjects.Add(null, null, null, "SGraph.Document");
                            //                Set scaleItem = refData.RichTextBox1.OLEObjects.item(refData.RichTextBox1.OLEObjects.Count - 1)
                            //                scaleItem.DoPaste
                            refData.RichTextBox1.SelectionStart++;
                            refData.RichTextBox1.SelectedText = "\r" + "\n" + "\r" + "\n";
                            refData.RichTextBox1.SelectionStart = refData.RichTextBox1.SelectionStart + 4;
                            //                       Debug.Print "Rule = " + CStr(nBufferNumber%), nBufferCode%, "  " + Mid(RSText![TXT_TXT_S], 1, 20)
                        }
                    }

                    if (nRet == 8)
                    {
                        if (numScaleSD == 1)
                        {
                        }
                        else if (numScaleSD == 2)
                        {
                            //                    PrepareTest
                        }
                        else if (numScaleSD == 3)
                        {
                        }
                        else if (numScaleSD == 4)
                        {
                        }
                    }

                    nRulesCount--;

                    if (bProgress)
                    {
                        // Display changes                        
                        refProgress.ProgressBar1.Value++;
                        //        If nRulesCount& Mod 10 = 0 Then
                        refProgress.Label4.Text = nRulesCount.ToString();
                        refProgress.Label5.Text = nRulesFound.ToString();
                        //        End If
                    }

                    System.Windows.Forms.Application.DoEvents();

                }

                if (bData)
                {
                    RSText.Close();
                }

                RSRules.Close();
                DBTest.Close();
                DBInterpr.Close();
            }
            catch
            {
                res = Interaction.MsgBox("Ошибка в интерпретируемых данных. Продолжить интерпретацию?", MsgBoxStyle.YesNo | MsgBoxStyle.Critical, null);
                if (res == MsgBoxResult.Yes)
                {
                    // Resume Next VBConversions Warning: Resume Next not supported in C#
                }
            }

        }

        public static short CompileRule(ref string strRule, short nPos)
        {
            short returnValue;
            string strSymID;
            short nSecondValue;
            short nFirstValue;
            short nValue;
            short nPosSpace = 0;
            float sThreedScaleValue = 0f;
            short nThreedScaleNum;
            string strThreedScaleName;
            float sSecondScaleValue = 0f;
            float sFirstScaleValue = 0f;
            short nSecondScaleNum;
            short nFirstScaleNum;
            string strSecondScaleName;
            string strFirstScaleName;
            short nRecord = 0;
            float sSecondScaleCompare;
            string strSecondOperation;
            float sFirstScaleCompare;
            string strFirstOperation;
            float sScaleValue = 0f;
            short nScaleNumber;
            float sScaleCompare;
            string strOperation;
            string strScaleName;
            string strAnswer;
            string strValue;
            short nAns;
            string strAddText;
            short nPosEnd;
            short nQuest;
            short nTestNumber;
            string strTestName;
            string strAuthenRuleCode;
            short nReturn;

            nReturn = 1;
            // Skip leading spaces
            if (nPos >= strRule.Length || Information.IsNumeric(strRule.Substring(nPos - 1, 1)) == false || strRule.Substring(nPos - 1, 1) == "!")
            {
                returnValue = nReturn;
                return returnValue;
            }

            // Get the Code and Analyze it
            nRuleCode = short.Parse(strRule.Substring(nPos - 1, 2));
            switch (nRuleCode)
            {

                // ===============================
                // Authentification
                case 1:
                    strAuthenRuleCode = strRule.Substring(3, 6);
                    break;

                // ===============================
                // Insert text
                case 2:
                    strTestName = strRule.Substring(nPos + 2 - 1, 3);
                    nTestNumber = Archive_Proc.daAFindTestNumber(numTester, strTestName);
                    if (nTestNumber >= 0)
                    {
                        nInsertTestNumber = nTestNumber;
                        nRange = 10;
                        nReturn = 7;
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;
                case 3:
                    strTestName = strRule.Substring(nPos + 2 - 1, 3);
                    nRange = short.Parse(strRule.Substring(nPos + 5 - 1, 3));
                    nTestNumber = Archive_Proc.daAFindTestNumber(numTester, strTestName);
                    if (nTestNumber >= 0)
                    {
                        nInsertTestNumber = nTestNumber;
                        nReturn = 7;
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;
                case 4:
                    strTestName = strRule.Substring(nPos + 2 - 1, 3);
                    nQuest = short.Parse(strRule.Substring(nPos + 5 - 1, 3));
                    nPosEnd = (short)((nPos + 8).ToString().IndexOf(strRule) + 1);
                    if (nPosEnd == 0)
                    {
                        nPosEnd = (short)(strRule.Length + 1);
                    }
                    strAddText = strRule.Substring(nPos + 8 - 1, nPosEnd - nPos - 8);

                    nAns = Archive_Proc.daAFindAnswerOnQuestionNumber(numTester, ref strTestName, nQuest);
                    if (nAns >= 0)
                    {
                        strAnswer = Scales_Proc.dbFindAnswerOnQuestion(ref strTestName, nQuest, nAns);
                        strRuleText = strAddText + " " + strAnswer + ".";
                        bRuleText = true;
                    }
                    break;

                // ===============================
                // Set current test
                case 10:
                    strTestSelected = strRule.Substring(nPos + 2 - 1, 3);
                    nTestNumber = Archive_Proc.daAFindTestNumber(numTester, strTestSelected);
                    if (nTestNumber >= 0)
                    {
                        nReturn = CompileRule(ref strRule, (short)(nPos + 5));
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;

                // ===============================
                // ScalesWindow and Value control
                case 21:
                    strScaleName = strRule.Substring(nPos + 2 - 1, 2);
                    strOperation = strRule.Substring(nPos + 4 - 1, 1);
                    sScaleCompare = (float)(Conversion.Val(strRule.Substring(nPos + 5 - 1, 5)));

                    nScaleNumber = Scales_Proc.daTFindScaleNumber(ref strTestSelected, ref strScaleName);
                    if (nScaleNumber >= 0)
                    {
                        nTestNumber = Archive_Proc.daAFindTestNumber(numTester, strTestSelected);
                        if (nTestNumber >= 0)
                        {
                            if (Archive_Proc.daAGetScale(nTestNumber, nScaleNumber, ref sScaleValue))
                            {

                                nReturn = DoCompareSingle(ref strOperation, sScaleValue, sScaleCompare);
                                if (nReturn == 1)
                                {
                                    nReturn = CompileRule(ref strRule, (short)(nPos + 10));
                                }
                            }
                            else
                            {
                                nReturn = 0;
                            }
                        }
                        else
                        {
                            nReturn = 0;
                        }
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;
                case 22:
                    strScaleName = strRule.Substring(nPos + 2 - 1, 2);
                    strFirstOperation = strRule.Substring(nPos + 4 - 1, 1);
                    sFirstScaleCompare = (float)(Conversion.Val(strRule.Substring(nPos + 5 - 1, 5)));
                    strSecondOperation = strRule.Substring(nPos + 10 - 1, 1);
                    sSecondScaleCompare = (float)(Conversion.Val(strRule.Substring(nPos + 11 - 1, 5)));

                    nScaleNumber = Scales_Proc.daTFindScaleNumber(ref strTestSelected, ref strScaleName);
                    if (nScaleNumber >= 0)
                    {
                        nTestNumber = Archive_Proc.daAFindTestNumber(numTester, strTestSelected);
                        if (nTestNumber >= 0)
                        {
                            if (Archive_Proc.daAGetScale(nTestNumber, nScaleNumber, ref sScaleValue))
                            {

                                nReturn = DoCompareSingle(ref strFirstOperation, sScaleValue, sFirstScaleCompare);
                                if (nReturn == 1)
                                {
                                    nReturn = DoCompareSingle(ref strSecondOperation, sScaleValue, sSecondScaleCompare);
                                    if (nRecord == 1)
                                    {
                                        nReturn = CompileRule(ref strRule, (short)(nPos + 16));
                                    }
                                }
                            }
                            else
                            {
                                nReturn = 0;
                            }
                        }
                        else
                        {
                            nReturn = 0;
                        }
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;

                // ===============================
                // ScalesWindow and ScalesWindow control
                case 31:
                    strFirstScaleName = strRule.Substring(nPos + 2 - 1, 2);
                    strOperation = strRule.Substring(nPos + 4 - 1, 1);
                    strSecondScaleName = strRule.Substring(nPos + 5 - 1, 2);

                    nFirstScaleNum = Scales_Proc.daTFindScaleNumber(ref strTestSelected, ref strFirstScaleName);
                    nSecondScaleNum = Scales_Proc.daTFindScaleNumber(ref strTestSelected, ref strSecondScaleName);
                    if (nFirstScaleNum >= 0 && nSecondScaleNum >= 0)
                    {
                        nTestNumber = Archive_Proc.daAFindTestNumber(numTester, strTestSelected);
                        if (nTestNumber >= 0)
                        {
                            if (Archive_Proc.daAGetScale(nTestNumber, nFirstScaleNum, ref sFirstScaleValue) && Archive_Proc.daAGetScale(nTestNumber, nSecondScaleNum, ref sSecondScaleValue))
                            {

                                nReturn = DoCompareSingle(ref strOperation, sFirstScaleValue, sSecondScaleValue);
                                if (nReturn == 1)
                                {
                                    nReturn = CompileRule(ref strRule, (short)(nPos + 7));
                                }
                            }
                            else
                            {
                                nReturn = 0;
                            }
                        }
                        else
                        {
                            nReturn = 0;
                        }
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;
                case 32:
                    strFirstScaleName = strRule.Substring(nPos + 2 - 1, 2);
                    strFirstOperation = strRule.Substring(nPos + 4 - 1, 1);
                    strSecondScaleName = strRule.Substring(nPos + 5 - 1, 2);
                    strSecondOperation = strRule.Substring(nPos + 7 - 1, 1);
                    strThreedScaleName = strRule.Substring(nPos + 8 - 1, 2);

                    nFirstScaleNum = Scales_Proc.daTFindScaleNumber(ref strTestSelected, ref strFirstScaleName);
                    nSecondScaleNum = Scales_Proc.daTFindScaleNumber(ref strTestSelected, ref strSecondScaleName);
                    nThreedScaleNum = Scales_Proc.daTFindScaleNumber(ref strTestSelected, ref strThreedScaleName);
                    if (nFirstScaleNum >= 0 && nSecondScaleNum >= 0 && nThreedScaleNum >= 0)
                    {
                        nTestNumber = Archive_Proc.daAFindTestNumber(numTester, strTestSelected);
                        if (nTestNumber >= 0)
                        {
                            if (Archive_Proc.daAGetScale(nTestNumber, nFirstScaleNum, ref sFirstScaleValue) && Archive_Proc.daAGetScale(nTestNumber, nSecondScaleNum, ref sSecondScaleValue) && Archive_Proc.daAGetScale(nTestNumber, nThreedScaleNum, ref sThreedScaleValue))
                            {

                                nReturn = DoCompareSingle(ref strFirstOperation, sFirstScaleValue, sSecondScaleValue);
                                if (nReturn == 1)
                                {
                                    nReturn = DoCompareSingle(ref strSecondOperation, sFirstScaleValue, sThreedScaleValue);
                                    if (nReturn == 1)
                                    {
                                        nReturn = CompileRule(ref strRule, (short)(nPosSpace + 10));
                                    }
                                }
                            }
                            else
                            {
                                nReturn = 0;
                            }
                        }
                        else
                        {
                            nReturn = 0;
                        }
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;

                // ===============================
                // Answers control
                case 41:
                    nQuest = short.Parse(strRule.Substring(nPos + 2 - 1, 3));
                    strOperation = strRule.Substring(nPos + 5 - 1, 1);
                    nValue = short.Parse(strRule.Substring(nPos + 6 - 1, Math.Min(3, strRule.Length - (nPos + 6 - 1))));

                    nAns = Archive_Proc.daAFindAnswerOnQuestionNumber(numTester, ref strTestSelected, nQuest);
                    if (nAns >= 0)
                    {
                        nReturn = DoCompareInt(ref strOperation, nAns, nValue);
                        if (nReturn == 1)
                        {
                            nReturn = CompileRule(ref strRule, (short)(nPos + 9));
                        }
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;
                case 42:
                    nQuest = short.Parse(strRule.Substring(nPos + 2 - 1, 3));
                    strFirstOperation = strRule.Substring(nPos + 5 - 1, 1);
                    nFirstValue = short.Parse(strRule.Substring(nPos + 6 - 1, 3));
                    strSecondOperation = strRule.Substring(nPos + 9 - 1, 1);
                    nSecondValue = short.Parse(strRule.Substring(nPos + 10 - 1, Math.Min(3, strRule.Length - (nPos + 10 - 1))));

                    nAns = Archive_Proc.daAFindAnswerOnQuestionNumber(numTester, ref strTestSelected, nQuest);
                    if (nAns >= 0)
                    {
                        nReturn = DoCompareInt(ref strFirstOperation, nAns, nFirstValue);
                        if (nReturn == 1)
                        {
                            nReturn = DoCompareInt(ref strSecondOperation, nAns, nSecondValue);
                            if (nRecord == 1)
                            {
                                nReturn = CompileRule(ref strRule, (short)(nPos + 13));
                            }
                        }
                    }
                    else
                    {
                        nReturn = 0;
                    }
                    break;

                // ===============================
                // Control files
                case 51:
                    break;
                case 52:
                    break;
                case 53:
                    break;

                case 59:
                    strTestName = strRule.Substring(nPos + 2 - 1, 3);
                    short testNum = Archive_Proc.daAFindTestNumber(numTester, strTestName);
                    if (testNum == -1)
                    {
                        if (Run_Proc.RunTestWizard(numTester, Archive_Proc.daArchiveRecord[numTester].strName, strTestName) == false)
                        {
                            nReturn = 0;
                        }
                    }
                    break;

                case 61:
                    break;
                case 62:
                    break;

                // ===============================
                // Semantic Differential
                case 71:
                    break;
                case 72:
                    strSymID = strRule.Substring(nPos + 2 - 1, 3);
                    anScale[0] = short.Parse(strRule.Substring(nPos + 5 - 1, 2));
                    anScale[1] = short.Parse(strRule.Substring(nPos + 7 - 1, 2));
                    numSDScales = 2;
                    nReturn = 8;
                    break;

                case 73:
                    break;
                case 74:
                    break;

                // ===============================
                // Routing commands
                case 91:
                    nReturn = 4;
                    nGoToRuleNumber = short.Parse(strRule.Substring(nPos + 2 - 1, 5));
                    break;
                case 92:
                    nReturn = 5;
                    break;
                case 99:
                    nReturn = 6;
                    break;

                // ===============================
                default:
                    Debug_Support.DebugIPrint("Неизвестный код правила");
                    break;
            }

            returnValue = nReturn;
            return returnValue;
        }

        private static short DoCompareSingle(ref string strOperation, float sFirstValue, float sSecondValue)
        {
            short returnValue;
            short nReturn;
            nReturn = 1;
            if (strOperation == "\u003C")
            {
                if (sFirstValue >= sSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u003E")
            {
                if (sFirstValue <= sSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u0010")
            {
                if (sFirstValue < sSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u0011")
            {
                if (sFirstValue > sSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u003D")
            {
                if (sFirstValue != sSecondValue)
                {
                    nReturn = 0;
                }
            }
            else
            {
                nReturn = 2;
                //            MsgBox ("Неизвестный символ сравнения " + strOperation$)
                Debug_Support.DebugIPrint("Неизвестный символ сравнения " + strOperation);
            }
            returnValue = nReturn;
            return returnValue;
        }

        private static short DoCompareInt(ref string strOperation, short nFirstValue, short nSecondValue)
        {
            short returnValue;
            short nReturn;
            nReturn = 1;
            if (strOperation == "\u003C")
            {
                if (nFirstValue >= nSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u003E")
            {
                if (nFirstValue <= nSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u0010")
            {
                if (nFirstValue < nSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u0011")
            {
                if (nFirstValue > nSecondValue)
                {
                    nReturn = 0;
                }
            }
            else if (strOperation == "\u003D")
            {
                if (nFirstValue != nSecondValue)
                {
                    nReturn = 0;
                }
            }
            else
            {
                nReturn = 2;
                //            MsgBox ("Неизвестный символ сравнения " + strOperation$)
                Debug_Support.DebugIPrint("Неизвестный символ сравнения " + strOperation);
            }
            returnValue = nReturn;
            return returnValue;
        }

        //UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        public static void SetText(string str_Renamed, ref System.Windows.Forms.RichTextBox re)
        {
            short Size = 7;
            int end_t;
            int start_t;
            string strOutLeft;
            string strOutRight;
            bool bold = false;
            bool italic = false;
            bool underline = false;
            string strFont;

            start_t = str_Renamed.IndexOf("{") + 1;
            if (start_t > 0)
            {
                end_t = str_Renamed.IndexOf("}") + 1;
                strFont = str_Renamed.Substring(start_t + 1 - 1, end_t - 2);
                Setting.ParseFont(ref strFont, ref Size, ref bold, ref italic, ref underline);
                strOutLeft = str_Renamed.Substring(0, start_t - 1);
                strOutRight = str_Renamed.Substring(str_Renamed.Length - str_Renamed.Length - end_t, str_Renamed.Length - end_t);
                re.SelectedText = strOutLeft;
                re.SelectionStart = re.SelectionStart + re.SelectionLength;
                Setting.SaveSelFont(re);
                ApplySelFont(re, strFont, Size, bold, italic, underline);
                re.SelectedText = strOutRight;
                re.SelectionStart = re.SelectionStart + re.SelectionLength;
                Setting.RestoreSelFont(re);
            }
            else
            {
                re.SelectedText = str_Renamed;
                re.SelectionStart = re.SelectionStart + re.SelectionLength;
            }
        }

        public static void ApplyFont(ref System.Drawing.Font f, string strFont, short Size, bool bold, bool italic, bool underline)
        {

            if (strFont.Substring(0, 1) == "#")
            {
                Setting.GetFontFromRegistry(strFont.Substring(strFont.Length - strFont.Length - 1, strFont.Length - 1), ref strFont, ref Size, ref bold, ref italic, ref underline);
            }

            f = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeName(f, strFont);
            if (Size != -1)
            {
                f = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeSize(f, Size);
            }
            f = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeBold(f, bold);
            f = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeItalic(f, italic);
            f = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeUnderline(f, underline);
        }

        public static void ApplySelFont(System.Windows.Forms.RichTextBox re, string strFont, short Size, bool bold, bool italic, bool underline)
        {

            if (strFont.Substring(0, 1) == "#")
            {
                Setting.GetFontFromRegistry(strFont.Substring(strFont.Length - strFont.Length - 1, strFont.Length - 1), ref strFont, ref Size, ref bold, ref italic, ref underline);
            }

            re.SelectionFont = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeName(re.SelectionFont, strFont);
            if (Size != -1)
            {
                re.SelectionFont = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeSize(re.SelectionFont, Size);
            }
            re.Font = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeBold(re.SelectionFont, bold);
            re.SelectionFont = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeItalic(re.SelectionFont, italic);
            re.SelectionFont = Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeUnderline(re.SelectionFont, underline);
        }

        public static void GetAverage(ref short[] daVal, short nColCount, short nRowCount, ref short[] Average)
        {
            short ValToDelete;
            short MaxIndex = 0;
            short J;
            short R;
            short N;
            short MaxCount = 0;
            short finish;
            short start;
            short K;
            short I;
            short[] daFreq;
            bool flag;

            // Not usual case
            if (nColCount == 1)
            {
                for (I = 0; I <= nRowCount - 1; I++)
                {
                    Average[I] = daVal[I];
                }
                return;
            }

            // Calculate frequency
            daFreq = new short[nColCount * nRowCount - 1 + 1];
            for (K = 0; K <= nRowCount - 1; K++)
            {
                flag = true;
                start = K;
                finish = K;
                do
                {
                    // Clear frequences
                    for (N = start; N <= finish; N++)
                    {
                        for (I = 0; I <= nColCount - 1; I++)
                        {
                            if (daVal[I * nRowCount + N] == -1)
                            {
                                daFreq[I * nRowCount + N] = 0;
                            }
                            else
                            {
                                daFreq[I * nRowCount + N] = 1;
                            }
                        }
                    }
                    for (N = start; N <= finish; N++)
                    {
                        // Get frequence value
                        for (I = 0; I <= nColCount - 1; I++)
                        {
                            if (daFreq[I * nRowCount + N] == 1)
                            {
                                for (R = start; R <= finish; R++)
                                {
                                    for (J = I; J <= nColCount - 1; J++)
                                    {
                                        if (N != R || J != I)
                                        {
                                            if (daVal[I * nRowCount + N] == daVal[J * nRowCount + R])
                                            {
                                                daFreq[I * nRowCount + N] = (short)(daFreq[I * nRowCount + N] + 1);
                                                daFreq[J * nRowCount + R] = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // Get maximum freqency and it count
                    for (N = start; N <= finish; N++)
                    {
                        for (I = 0; I <= nColCount - 1; I++)
                        {
                            if (N == start && I == 0)
                            {
                                MaxIndex = start;
                                MaxCount = 1;
                            }
                            else if (daFreq[I * nRowCount + N] > daFreq[MaxIndex])
                            {
                                MaxIndex = (short)(I * nRowCount + N);
                                MaxCount = 1;
                            }
                            else if (daFreq[I * nRowCount + N] == daFreq[MaxIndex])
                            {
                                MaxCount++;
                            }
                        }
                    }
                    // Return average
                    if (MaxCount == 1)
                    {
                        Average[K] = daVal[MaxIndex];
                        ValToDelete = daVal[MaxIndex];
                        for (N = 0; N <= nRowCount - 1; N++)
                        {
                            for (I = 0; I <= nColCount - 1; I++)
                            {
                                if (ValToDelete == daVal[I * nRowCount + N])
                                {
                                    daVal[I * nRowCount + N] = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (start == 0 && finish == nRowCount - 1)
                        {
                            // Error
                            Debug_Support.DebugPrint("Ошибка в GetAverage");
                            MessageBox.Show("Ошибка");
                            return;
                        }
                        if (flag == true && start > 0)
                        {
                            start--;
                        }
                        else if (finish < nRowCount - 1)
                        {
                            finish++;
                        }
                        if (flag == true)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                } while (!(MaxCount == 1));
            }
        }

        public static short GetAverage1(short[] daVal, short nCount)
        {
            short returnValue = 0;
            short Number;
            short MaxCount;
            short Max;
            short J;
            short I;
            short[] daFreq;

            if (nCount == 1)
            {
                returnValue = daVal[0];
                return returnValue;
            }
            // Calculate frequency
            daFreq = new short[nCount - 1 + 1];
            for (I = 0; I <= nCount - 1; I++)
            {
                daFreq[I] = 1;
            }

            for (I = 0; I <= nCount - 1; I++)
            {
                if (daFreq[I] == 1)
                {
                    for (J = (short)(I + 1); J <= nCount - 1; J++)
                    {
                        if (daVal[I] == daVal[J])
                        {
                            daFreq[I] = (short)(daFreq[I] + 1);
                            daFreq[J] = 0;
                        }
                    }
                }
            }
            // Get maximum freqency and it count
            Max = 0;
            MaxCount = 1;
            for (I = 1; I <= nCount - 1; I++)
            {
                if (daFreq[I] > daFreq[Max])
                {
                    Max = I;
                    MaxCount = 1;
                }
                else if (daFreq[I] == daFreq[Max])
                {
                    MaxCount++;
                }
            }
            // Return average
            if (MaxCount == 1)
            {
                returnValue = daVal[Max];
                return returnValue;
            }
            Number = (short)Math.Round((MaxCount - 1 + 1) * VBMath.Rnd() + 1);
            for (I = 0; I <= nCount - 1; I++)
            {
                if (daFreq[I] == daFreq[Max])
                {
                    Number--;
                    if (Number == 0)
                    {
                        returnValue = daVal[I];
                    }
                }
            }
            return returnValue;
        }
    }
}
