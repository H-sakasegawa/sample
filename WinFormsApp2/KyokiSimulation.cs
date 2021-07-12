using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    class KyokiKansi 
    { 
        public KyokiKansi( Kansi _kansi)
        {
            kansi = _kansi.Clone();
            bChange = false;
        }
        public KyokiKansi Clone()
        {
            // Object型で返ってくるのでキャストが必要
            KyokiKansi clone = (KyokiKansi)MemberwiseClone();
            //kansiは参照になってしまうのでkansiのクローンを作成する
            clone.kansi = this.kansi.Clone();

            return clone;
        }

        public string kan
        {
            get { return kansi.kan; }
            set { kansi.kan = value; bChange = true; }
        }
        public string si
        {
            get { return kansi.si; }
            set { kansi.si = value; bChange = true; }
        }

        public Kansi kansi;
        public bool bChange;    //変化有無フラグ
    }
    /// <summary>
    /// 虚気パターン解析用 干支情報
    /// </summary>
    class KansiInfo
    {
        public KansiInfo(KyokiKansi[] _aryKansi)
        {
            bCirculation = false;
            aryKansi = new KyokiKansi[_aryKansi.Length];
            for (int i = 0; i < aryKansi.Length; i++)
            {
                aryKansi[i] = _aryKansi[i].Clone();
            }
        }

        public bool IsSame(KyokiKansi[] _aryKansi)
        {
            for (int i = 0; i < aryKansi.Length; i++)
            {
                if (!aryKansi[i].kansi.IsSame(_aryKansi[i].kansi))
                {
                    return false;
                }
            }
            return true;
        }

        public KyokiKansi[] aryKansi = null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="getuunKansi"></param>
        /// <param name="nenunKansi"></param>
        /// <param name="taiunKansi"></param>
        /// <param name="bDispGetuun"></param>
        /// <returns>0..循環無し  1..循環あり</returns>
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

            KyokiKansi[] aryKansi = new KyokiKansi[] { 
                                        new KyokiKansi(_getuunKansi), new KyokiKansi(_nenunKansi), new KyokiKansi(_taiunKansi),
                                        new KyokiKansi(_nikkansi), new KyokiKansi(_gekkansi), new KyokiKansi(_nenkansi)
                                                    };

            lstKansPattern.Clear();

            return  DoSim(aryKansi,  bDispGetuun);
        }

        /// <summary>
        /// パターン解析開始
        /// </summary>
        /// <param name="aryKansi"></param>
        /// <param name="bDispGetuun"></param>
        /// <returns>0..循環無し  1..循環あり</returns>
        private int DoSim(KyokiKansi[] aryKansi,  bool bDispGetuun  )
        {
            int rc = 0;

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

                        rc = 1; //循環あり
                    }
                    //前回発生したパターンと同じものがでてきた、⇒ここで終了
                    return rc;
                }
            }


            //再帰呼び出し
            return DoSim(aryKansi,  bDispGetuun);
        }

        /// <summary>
        /// 干合 テーブルによる干文字の虚気文字変換
        /// </summary>
        /// <param name="nikkansi">日干支</param>
        /// <param name="gekkansi">月干支</param>
        /// <param name="nenkansi">年干支</param>
        public void RefrectKangou(KyokiKansi nikkansi, KyokiKansi gekkansi, KyokiKansi nenkansi)
        {
            var tblMng = TableMng.GetTblManage();

            //オリジナルをコピー ⇒変換判定はオリジナルで比較
            KyokiKansi _nikkansi = nikkansi.Clone();
            KyokiKansi _nenkansi = nenkansi.Clone();
            KyokiKansi _gekkansi = gekkansi.Clone();


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
        public void RefrectKangou(KyokiKansi nikkansi, KyokiKansi gekkansi, KyokiKansi nenkansi,
                                  KyokiKansi getuunKansi, KyokiKansi nenunKansi, KyokiKansi taiunKansi,
                                  bool bDispGetuun
           )
        {
            var tblMng = TableMng.GetTblManage();

            //オリジナルをコピー ⇒変換判定はオリジナルで比較
            KyokiKansi _nikkansi = nikkansi.Clone();
            KyokiKansi _gekkansi = gekkansi.Clone();
            KyokiKansi _nenkansi = nenkansi.Clone();

            KyokiKansi _getuunKansi = getuunKansi.Clone();
            KyokiKansi _nenunKansi = nenunKansi.Clone();
            KyokiKansi _taiunKansi = taiunKansi.Clone();

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
