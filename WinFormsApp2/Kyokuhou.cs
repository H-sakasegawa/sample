using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{

    /// <summary>
    /// 局法 クラス
    /// </summary>
    class Kyokuhou
    {

        TableMng tblMng = TableMng.GetTblManage();
        public class Result
        {
            public Result(string _name, int _cnt=1)
            {
                name = _name;
                count = _cnt;
            }
            public string name;
            public int count;
        }
        //                | D |
        //             ---+---+----
        //JudaiShusei  A  | B | C   
        //             ---+---+----
        //                | E |

        public const string SuigyakuKyoku = "推逆局";

        //==================================================
        //局法（凶運）
        //==================================================
        public List<Result> GetKyouUn(Person person)
        {
            List<Result> lstResult = new List<Result>();

            int resultCnt = 0;

            string[] patternVer = new string[]
            {
                person.judaiShuseiB.name,
                person.judaiShuseiD.name,
                person.judaiShuseiE.name
            };
            string[] patternHor = new string[]
            {
                person.judaiShuseiA.name,
                person.judaiShuseiB.name,
                person.judaiShuseiC.name
            };


            //推逆局 判定（D、B、E）
            List<string> target = new List<string> { "鳳閣星", "調舒星", "龍高星", "玉堂星" };
            resultCnt = CheckKyouUn(target, patternVer);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("推逆局", resultCnt));
            }
            //円推局 判定（A、B、C）
            resultCnt = CheckKyouUn(target, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("円推局", resultCnt));
            }

            //生殺局 判定（D、B、E）
            target = new List<string> { "鳳閣星", "調舒星", "車騎星", "牽牛星" };
            resultCnt = CheckKyouUn(target, patternVer);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("生殺局", resultCnt));
            }
            //殺局 判定（A、B、C）
            resultCnt = CheckKyouUn(target, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("円推局", resultCnt));
            }

            //井乱局 判定（D、B、E）
            target = new List<string> { "車騎星", "牽牛星", "貫索星", "石門星" };
            resultCnt = CheckKyouUn(target, patternVer);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("井乱局", resultCnt));
            }
            //乱命局 判定（A、B、C）
            resultCnt = CheckKyouUn(target, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("乱命局", resultCnt));
            }

            //破財局 判定（D、B、E）
            target = new List<string> { "貫索星", "石門星", "禄存星", "司禄星" };
            resultCnt = CheckKyouUn(target, patternVer);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("破財局", resultCnt));
            }
            //曲財局 判定（A、B、C）
            resultCnt = CheckKyouUn(target, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("乱命局", resultCnt));
            }

            //地財局 判定（D、B、E）
            target = new List<string> { "禄存星", "司禄星", "龍高星", "玉堂星" };
            resultCnt = CheckKyouUn(target, patternVer);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("地財局", resultCnt));
            }
            //叉財局 判定（A、B、C）
            resultCnt = CheckKyouUn(target, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("叉財局", resultCnt));
            }
            return lstResult;
        }

        private int CheckKyouUn(List<string> target, string[] pattern)
        {
            int resultCnt = 0;
            for (int i = 0; i < pattern.Length - 1; i++)
            {
                if (!target.Contains(pattern[i])) continue;

                for (int j = i + 1; j < pattern.Length; j++)
                {
                    if (!target.Contains(pattern[j])) continue;

                    //pattern[i]のgogyoを取得
                    string attr1 = tblMng.juudaiShusei.GetGogyo(pattern[i]);
                    string attr2 = tblMng.juudaiShusei.GetGogyo(pattern[j]);
                    if (attr1 != attr2)
                    {
                        //ここで２つの五行属性が異なるということは、相剋の関係
                        resultCnt++;
                    }

                }
            }
            return resultCnt;

        }

        //==================================================
        //局法（幸運）
        //==================================================
        public List<Result> GetKouUn(Person person)
        {
            List<Result> lstResult = new List<Result>();

            int resultCnt = 0;

            string[] patternVer = new string[]
            {
                person.judaiShuseiB.name,
                person.judaiShuseiD.name,
                person.judaiShuseiE.name
            };
            string[] patternHor = new string[]
            {
                person.judaiShuseiA.name,
                person.judaiShuseiB.name,
                person.judaiShuseiC.name
            };

            //縦またば横軸で判定
            List<string> lstopShusei = new List<string> { "石門星", "貫索星" };
            resultCnt = CheckKouUn_VerOrHor(lstopShusei, patternVer, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("芳醇局", resultCnt));
            }

            lstopShusei = new List<string> { "鳳閣星", "調舒星" };
            resultCnt = CheckKouUn_VerOrHor(lstopShusei, patternVer, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("八寿局", resultCnt));
            }

            lstopShusei = new List<string> { "禄存星", "司禄星" };
            resultCnt = CheckKouUn_VerOrHor(lstopShusei, patternVer, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("三麗局", resultCnt));
            }

            lstopShusei = new List<string> { "車騎星", "牽牛星" };
            resultCnt = CheckKouUn_VerOrHor(lstopShusei, patternVer, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("相法局", resultCnt));
            }

            lstopShusei = new List<string> { "龍高星","玉堂星" };
            resultCnt = CheckKouUn_VerOrHor(lstopShusei, patternVer, patternHor);
            if (resultCnt > 0)
            {
                lstResult.Add(new Result("玄流局", resultCnt));
            }
            //----------------------------------------------------------------

            //縦と横軸の両方成立で判定
            lstopShusei = new List<string> { "貫索星","石門星" };
            bool bResult = CheckKouUn_VerAndHor(lstopShusei, patternVer, patternHor);
            if (bResult)
            {
                lstResult.Add(new Result("劫法局"));
            }

            lstopShusei = new List<string> { "鳳閣星", "調舒星" };
            bResult = CheckKouUn_VerAndHor(lstopShusei, patternVer, patternHor);
            if (bResult)
            {
                lstResult.Add(new Result("天華局"));
            }
            lstopShusei = new List<string> { "禄存星","司禄星" };
            bResult = CheckKouUn_VerAndHor(lstopShusei, patternVer, patternHor);
            if (bResult)
            {
                lstResult.Add(new Result("連財局"));
            }

            lstopShusei = new List<string> { "禄存星", "司禄星" };
            bResult = CheckKouUn_VerAndHor(lstopShusei, patternVer, patternHor);
            if (bResult)
            {
                lstResult.Add(new Result("連財局"));
            }

            lstopShusei = new List<string> { "車騎星", "牽牛星" };
            bResult = CheckKouUn_VerAndHor(lstopShusei, patternVer, patternHor);
            if (bResult)
            {
                lstResult.Add(new Result("法官局"));
            }
            lstopShusei = new List<string> { "龍高星","玉堂星" };
            bResult = CheckKouUn_VerAndHor(lstopShusei, patternVer, patternHor);
            if (bResult)
            {
                lstResult.Add(new Result("命龍局"));
            }

            return lstResult;
        }
        private int CheckKouUn_VerOrHor(List<string> stopShusei, string[] targetVer, string[] targetHor)
        {
            int cnt = CheckKouUn_Sub(stopShusei, targetVer);
            cnt += CheckKouUn_Sub(stopShusei, targetHor);
            return cnt;
        }
        private bool CheckKouUn_VerAndHor(List<string> stopShusei, string[] targetVer, string[] targetHor)
        {
            int cnt1 = CheckKouUn_Sub(stopShusei, targetVer);
            int cnt2 =  CheckKouUn_Sub(stopShusei, targetHor);
            return ( cnt1>0 && cnt2>0);
        }
        private int CheckKouUn_Sub(List<string> stopShusei, string[] target)
        {
            int resultCnt = 0;
            //３マスを A ｜B ｜C とするなら
            //     A→B→C
            //     A→C→B
            //     C→A→B
            //     B→A→C
            //　　　： 
            //　　のように、マスの順番の組み合わせで各マス間
            //において相生の関係が続く場合で、終端に
            //指定された星がある場合は成立と判断
            var relation = tblMng.gogyouAttrRelationshipTbl;
            for (int i = 0; i < target.Length; i++)
            {
                string attr1 = tblMng.juudaiShusei.GetGogyo(target[i]);
                for (int j = 0; j < target.Length; j++)
                {
                    if (i == j) continue;

                    string attr2 = tblMng.juudaiShusei.GetGogyo(target[j]);
                    if (relation.IsCreate(attr1, attr2))
                    {   //相生(A→B)

                        //(B→C）は相生か？
                        for (int k = 0; k < target.Length; k++)
                        {
                            if (k == i || k == j) continue;
                            //終端主星をチェック
                            if (stopShusei.Contains(target[k])) continue;

                            string attr3 = tblMng.juudaiShusei.GetGogyo(target[k]);
                            if (relation.IsCreate(attr2, attr3))
                            {   //相生(B→C)
                                resultCnt++;
                                return resultCnt;
                            }
                        }
                    }
                }
            }
            //この段階で成立したら以降は検索しない
            if (resultCnt > 0) return resultCnt;

            //上記が成立しなかったら、２つのマス間（From - To） 
            //　で相生の関係があるかをチェック
            //相生の関係があればToに終端として指定された星があるかをチェック。 
            //　あれば、その２つのマスに対し成立フラグを設定。 
            //　上記のチェックをすべてのマスの組み合わせで実施し、 
            //　すべてのマスに成立フラグが設定されたら、成立と判断
            //パターンとしては以下の組み合わせ。
            //1)A→B←C
            //2)A→C、B→C
            //3)B→A、B→C
            //4)C→B、C→A
            int chkMatrix = 0;
            for (int i = 0; i < target.Length; i++)
            {
                string attr1 = tblMng.juudaiShusei.GetGogyo(target[i]);
                for (int j = 0; j < target.Length; j++)
                {
                    if (i == j) continue;

                    //終端主星をチェック
                    if (stopShusei.Contains(target[j])) continue;

                    string attr2 = tblMng.juudaiShusei.GetGogyo(target[j]);
                    if (relation.IsCreate(attr1, attr2))
                    {   //相生
                        chkMatrix |= 0x0001 << i;
                        chkMatrix |= 0x0001 << j;

                        if (chkMatrix == 0x0007) resultCnt++;
                    }
                }
            }

            //不成立
            return resultCnt;

        }

        //==================================================
        //別格
        //==================================================
        public List<Result> GetBekkakku(Person person)
        {
            List<Result> lstResult = new List<Result>();
            string[] patternAll = new string[]
            {
                person.judaiShuseiA.name,
                person.judaiShuseiB.name,
                person.judaiShuseiC.name,
                person.judaiShuseiD.name,
                person.judaiShuseiE.name
            };
            //全て同じ星で構成されているか？
            bool bResult = true;
            for(int i=1; i<patternAll.Length; i++)
            {
                if(patternAll[0] != patternAll[i])
                {
                    bResult = false;
                    break;
                }
            }
            if (bResult)
            {
                lstResult.Add(new Result("鳳蘭局"));
            }

            return lstResult;
        }

        //==================================================
        //特殊五局
        //==================================================
        public List<Result> GetTokushuGokyoku(Person person)
        {
            List<Result> lstResult = new List<Result>();
            string[] patternAll = new string[]
            {
                person.judaiShuseiA.name,
                person.judaiShuseiB.name,
                person.judaiShuseiC.name,
                person.judaiShuseiD.name,
                person.judaiShuseiE.name
            };

            //指定された星だけで構成されているもの
            List<string> lstopShusei = new List<string> { "石門星", "貫索星" };
            if(checkTokushuGokyoku(lstopShusei, patternAll))
            { 
                lstResult.Add(new Result("劫局"));
            }

            lstopShusei = new List<string> { "鳳閣星", "調舒星" };
            if (checkTokushuGokyoku(lstopShusei, patternAll))
            {
                lstResult.Add(new Result("食局"));
            }

            lstopShusei = new List<string> { "禄存星", "司禄星" };
            if (checkTokushuGokyoku(lstopShusei, patternAll))
            {
                lstResult.Add(new Result("財局"));
            }

            lstopShusei = new List<string> { "車騎星", "牽牛星" };
            if (checkTokushuGokyoku(lstopShusei, patternAll))
            {
                lstResult.Add(new Result("官局"));
            }

            lstopShusei = new List<string> { "龍高星","玉堂星" };
            if (checkTokushuGokyoku(lstopShusei, patternAll))
            {
                lstResult.Add(new Result("印局"));
            }

            return lstResult;
        }

        bool checkTokushuGokyoku( List<string> shusei, string[] targetAll)
        {
            bool bResult = true;
            for (int i = 0; i < targetAll.Length; i++)
            {
                if (!shusei.Contains(targetAll[i]))
                {
                    bResult = false;
                    break;
                }
            }
            return bResult;
        }

    }
}
