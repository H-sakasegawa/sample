using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 虚気パターン解析用 干支情報
    /// </summary>
    class KansiInfo
    {
        public KansiInfo( Kansi[] _aryKansi)
        {
            bCirculation = false;
            aryKansi = new Kansi[_aryKansi.Length];
            for (int i = 0; i < aryKansi.Length; i++)
            {
                aryKansi[i] = _aryKansi[i].Clone();
            }
        }

        public bool IsSame(Kansi[] _aryKansi)
        {
            for (int i = 0; i < aryKansi.Length; i++)
            {
                if (!aryKansi[i].IsSame(_aryKansi[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public Kansi[] aryKansi = new Kansi[6];
        public bool bCirculation;
    }

    /// <summary>
    /// 虚気 変換シミュレーション
    /// </summary>
    class KyokiSimulation
    {
        public List<KansiInfo> lstKansPattern = new List<KansiInfo>();
        public Kansi[] aryKansiOrg;

        const int MaxPatternNum = 20;

        public int Simulation( Person person,
                                Kansi getuunKansi,
                                Kansi nenunKansi,
                                Kansi taiunKansi,
                                bool bDispGetuun
                                )
        {
            aryKansiOrg = new Kansi[]
            {
                getuunKansi, nenunKansi, taiunKansi,
                person.nikkansi, person.nenkansi, person.gekkansi,
            };

            //シミュレーションでPersonが保持している情報が書き変わってしまわないようCloseを作成
            Kansi _nikkansi = person.nikkansi.Clone();
            Kansi _nenkansi = person.nenkansi.Clone();
            Kansi _gekkansi = person.gekkansi.Clone();

            Kansi _getuunKansi = getuunKansi.Clone();
            Kansi _nenunKansi = nenunKansi.Clone();
            Kansi _taiunKansi = taiunKansi.Clone();

            Kansi[] aryKansi = new Kansi[] { _getuunKansi, _nenunKansi, _taiunKansi, _nikkansi, _gekkansi, _nenkansi  };

            lstKansPattern.Clear();

            DoSim(aryKansi,  bDispGetuun);
            return 0;
        }

        /// <summary>
        /// パターン解析開始
        /// </summary>
        /// <param name="aryKansi"></param>
        /// <param name="bDispGetuun"></param>
        /// <returns></returns>
        private int DoSim(Kansi[] aryKansi,  bool bDispGetuun  )
        {

            lstKansPattern.Add(new KansiInfo(aryKansi));

            if( lstKansPattern.Count >= MaxPatternNum)
            {
                return 0;
            }

            //現在の干支属性情報をバックアップ
 
            RefrectKangou(  aryKansi[(int)Const.enumKansiItemID.NIKKANSI],
                            aryKansi[(int)Const.enumKansiItemID.GEKKANSI],
                            aryKansi[(int)Const.enumKansiItemID.NENKANSI],
                            aryKansi[(int)Const.enumKansiItemID.GETUUN],
                            aryKansi[(int)Const.enumKansiItemID.NENUN],
                            aryKansi[(int)Const.enumKansiItemID.TAIUN],
                            bDispGetuun
                          );

            for( int i=0; i< lstKansPattern.Count; i++)
            {
                var item = lstKansPattern[i];

                if (item.IsSame(aryKansi))
                {
                    //最後の項目と一緒なら、変化が発生しなかったということ。
                    if (i != lstKansPattern.Count-1)
                    {
                        item.bCirculation = true; //循環ポイントにマーク
                        var lastItem = new KansiInfo(aryKansi);
                        lastItem.bCirculation = true;
                        lstKansPattern.Add(lastItem);
                    }
                    //前回発生したパターンと同じものがでてきた、⇒ここで終了
                    return 0;
                }
            }


            //再帰呼び出し
            DoSim(aryKansi,  bDispGetuun);

            return 0;
        }

        /// <summary>
        /// 干合 テーブルによる干文字の虚気文字変換
        /// </summary>
        /// <param name="nikkansi">日干支</param>
        /// <param name="gekkansi">月干支</param>
        /// <param name="nenkansi">年干支</param>
        public void RefrectKangou(Kansi nikkansi, Kansi gekkansi, Kansi nenkansi)
        {
            var tblMng = TableMng.GetTblManage();

            //オリジナルをコピー ⇒変換判定はオリジナルで比較
            Kansi _nikkansi = nikkansi.Clone();
            Kansi _nenkansi = nenkansi.Clone();
            Kansi _gekkansi = gekkansi.Clone();


            //================================================
            //干合
            //================================================
            //日（干） - 月（干）
            var kyoki = tblMng.kangouTbl.GetKyoki(_nikkansi.kan, _gekkansi.kan);
            if (kyoki != null)
            {
                nikkansi.kan = kyoki[0];
                gekkansi.kan = kyoki[1];
            }
            //日（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_nikkansi.kan, _nenkansi.kan);
            if (kyoki != null)
            {
                nikkansi.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }
            //月（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_gekkansi.kan, _nenkansi.kan);
            if (kyoki != null)
            {
                gekkansi.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }
        }
        /// <summary>
        /// 干合 テーブルによる干文字の虚気文字変換
        /// </summary>
        /// <param name="nikkansi">日干支</param>
        /// <param name="gekkansi">月干支</param>
        /// <param name="nenkansi">年干支</param>
        /// <param name="getuunKansi">月運干支</param>
        /// <param name="nenunKansi">年運干支</param>
        /// <param name="taiunKansi">大運干支</param>
        /// <param name="bDispGetuun">月運表示・非表示フラグ</param>
        public void RefrectKangou(Kansi nikkansi, Kansi gekkansi, Kansi nenkansi,
                                  Kansi getuunKansi, Kansi nenunKansi, Kansi taiunKansi,
                                  bool bDispGetuun
           )
        {
            var tblMng = TableMng.GetTblManage();

            //オリジナルをコピー ⇒変換判定はオリジナルで比較
            Kansi _nikkansi = nikkansi.Clone();
            Kansi _gekkansi = gekkansi.Clone();
            Kansi _nenkansi = nenkansi.Clone();

            Kansi _getuunKansi = getuunKansi.Clone();
            Kansi _nenunKansi = nenunKansi.Clone();
            Kansi _taiunKansi = taiunKansi.Clone();

            string[] kyoki;
            //宿命カラー設定
            RefrectKangou(nikkansi, gekkansi, nenkansi);


            //月運、年運、大運 カラー設定
            //================================================
            //干合
            //================================================
            //----------------------------------
            // 月運 →＊
            //----------------------------------
            if (bDispGetuun)
            {
                //月運（干） - 年運（干）
                kyoki = tblMng.kangouTbl.GetKyoki(_getuunKansi.kan, _nenunKansi.kan);
                if (kyoki != null)
                {
                    getuunKansi.kan = kyoki[0];
                    nenunKansi.kan = kyoki[1];
                }
                //月運（干） - 大運（干）
                kyoki = tblMng.kangouTbl.GetKyoki(_getuunKansi.kan, _taiunKansi.kan);
                if (kyoki != null)
                {
                    getuunKansi.kan = kyoki[0];
                    taiunKansi.kan = kyoki[1];
                }
                //月運（干） - 日（干）
                kyoki = tblMng.kangouTbl.GetKyoki(_getuunKansi.kan, _nikkansi.kan);
                if (kyoki != null)
                {
                    getuunKansi.kan = kyoki[0];
                    _nikkansi.kan = kyoki[1];
                }
                //月運（干） - 月（干）
                kyoki = tblMng.kangouTbl.GetKyoki(_getuunKansi.kan, _gekkansi.kan);
                if (kyoki != null)
                {
                    getuunKansi.kan = kyoki[0];
                    gekkansi.kan = kyoki[1];
                }
                //月運（干） - 年（干）
                kyoki = tblMng.kangouTbl.GetKyoki(_getuunKansi.kan, _nenkansi.kan);
                if (kyoki != null)
                {
                    getuunKansi.kan = kyoki[0];
                    nenkansi.kan = kyoki[1];
                }
            }
            //----------------------------------
            // 年運 →＊
            //----------------------------------
            //年運（干） - 大運（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_nenunKansi.kan, _taiunKansi.kan);
            if (kyoki != null)
            {
                nenunKansi.kan = kyoki[0];
                taiunKansi.kan = kyoki[1];
            }
            //年運（干） - 日（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_nenunKansi.kan, _nikkansi.kan);
            if (kyoki != null)
            {
                nenunKansi.kan = kyoki[0];
                nikkansi.kan = kyoki[1];
            }
            //年運（干） - 月（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_nenunKansi.kan, _gekkansi.kan);
            if (kyoki != null)
            {
                nenunKansi.kan = kyoki[0];
                gekkansi.kan = kyoki[1];
            }
            //年運（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_nenunKansi.kan, _nenkansi.kan);
            if (kyoki != null)
            {
                nenunKansi.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }
            //----------------------------------
            // 大運 →＊
            //----------------------------------
            //大運（干） - 日（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_taiunKansi.kan, _nikkansi.kan);
            if (kyoki != null)
            {
                taiunKansi.kan = kyoki[0];
                nikkansi.kan = kyoki[1];
            }
            //大運（干） - 月（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_taiunKansi.kan, _gekkansi.kan);
            if (kyoki != null)
            {
                taiunKansi.kan = kyoki[0];
                gekkansi.kan = kyoki[1];
            }
            //大運（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(_taiunKansi.kan, nenkansi.kan);
            if (kyoki != null)
            {
                taiunKansi.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }

        }
    }
}
