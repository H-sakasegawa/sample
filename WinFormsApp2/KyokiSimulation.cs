using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{

    class KansiInfo
    {
        public KansiInfo( Kansi[] _aryKansi)
        {
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
    }

    /// <summary>
    /// 虚気 変換シミュレーション
    /// </summary>
    class KyokiSimulation
    {
        public List<KansiInfo> lstKansPattern = new List<KansiInfo>();
        public Kansi[] aryKansiOrg;

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

            DoSim(aryKansi,  bDispGetuun);
            return 0;
        }

        private int DoSim(Kansi[] aryKansi,  bool bDispGetuun  )
        {

            lstKansPattern.Add(new KansiInfo(aryKansi));

            //現在の干支属性情報をバックアップ
 
            RefrectKangou(  aryKansi[(int)Const.enumKansiItemID.NIKKANSI],
                            aryKansi[(int)Const.enumKansiItemID.GEKKANSI],
                            aryKansi[(int)Const.enumKansiItemID.NENKANSI],
                            aryKansi[(int)Const.enumKansiItemID.GETUUN],
                            aryKansi[(int)Const.enumKansiItemID.NENUN],
                            aryKansi[(int)Const.enumKansiItemID.TAIUN],
                            bDispGetuun
                          );

            foreach(var item in lstKansPattern)
            {
                if (item.IsSame(aryKansi))
                {
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
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        public void RefrectKangou(Kansi nikkansi, Kansi gekkansi, Kansi nenkansi)
        {
            var tblMng = TableMng.GetTblManage();

            //================================================
            //干合
            //================================================
            //日（干） - 月（干）
            var kyoki = tblMng.kangouTbl.GetKyoki(nikkansi.kan, gekkansi.kan);
            if (kyoki != null)
            {
                nikkansi.kan = kyoki[0];
                gekkansi.kan = kyoki[1];
            }
            //日（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(nikkansi.kan, nenkansi.kan);
            if (kyoki != null)
            {
                nikkansi.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }
            //月（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(gekkansi.kan, nenkansi.kan);
            if (kyoki != null)
            {
                gekkansi.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }
        }
        /// <summary>
        /// 干合 テーブルによる干文字の虚気文字変換
        /// </summary>
        /// <param name="colorNikkansi"></param>
        /// <param name="colorGekkansi"></param>
        /// <param name="colorNenkansi"></param>
        /// <param name="colorGetuun"></param>
        /// <param name="colorNenun"></param>
        /// <param name="colorTaiun"></param>
        /// <param name="kansiGetuun"></param>
        /// <param name="kansiNenun"></param>
        /// <param name="kansiTaiun"></param>
        /// <param name="bDispGetuun"></param>
        public void RefrectKangou(Kansi nikkansi, Kansi gekkansi, Kansi nenkansi,
                                  Kansi kansiGetuun, Kansi kansiNenun, Kansi kansiTaiun,
                                  bool bDispGetuun
           )
        {
            var tblMng = TableMng.GetTblManage();

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
                kyoki = tblMng.kangouTbl.GetKyoki(kansiGetuun.kan, kansiNenun.kan);
                if (kyoki != null)
                {
                    kansiGetuun.kan = kyoki[0];
                    kansiNenun.kan = kyoki[1];
                }
                //月運（干） - 大運（干）
                kyoki = tblMng.kangouTbl.GetKyoki(kansiGetuun.kan, kansiTaiun.kan);
                if (kyoki != null)
                {
                    kansiGetuun.kan = kyoki[0];
                    kansiTaiun.kan = kyoki[1];
                }
                //月運（干） - 日（干）
                kyoki = tblMng.kangouTbl.GetKyoki(kansiGetuun.kan, nikkansi.kan);
                if (kyoki != null)
                {
                    kansiGetuun.kan = kyoki[0];
                    nikkansi.kan = kyoki[1];
                }
                //月運（干） - 月（干）
                kyoki = tblMng.kangouTbl.GetKyoki(kansiGetuun.kan, gekkansi.kan);
                if (kyoki != null)
                {
                    kansiGetuun.kan = kyoki[0];
                    gekkansi.kan = kyoki[1];
                }
                //月運（干） - 年（干）
                kyoki = tblMng.kangouTbl.GetKyoki(kansiGetuun.kan, nenkansi.kan);
                if (kyoki != null)
                {
                    kansiGetuun.kan = kyoki[0];
                    nenkansi.kan = kyoki[1];
                }
            }
            //----------------------------------
            // 年運 →＊
            //----------------------------------
            //年運（干） - 大運（干）
            kyoki = tblMng.kangouTbl.GetKyoki(kansiNenun.kan, kansiTaiun.kan);
            if (kyoki != null)
            {
                kansiNenun.kan = kyoki[0];
                kansiTaiun.kan = kyoki[1];
            }
            //年運（干） - 日（干）
            kyoki = tblMng.kangouTbl.GetKyoki(kansiNenun.kan, nikkansi.kan);
            if (kyoki != null)
            {
                kansiNenun.kan = kyoki[0];
                nikkansi.kan = kyoki[1];
            }
            //年運（干） - 月（干）
            kyoki = tblMng.kangouTbl.GetKyoki(kansiNenun.kan, gekkansi.kan);
            if (kyoki != null)
            {
                kansiNenun.kan = kyoki[0];
                gekkansi.kan = kyoki[1];
            }
            //年運（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(kansiNenun.kan, nenkansi.kan);
            if (kyoki != null)
            {
                kansiNenun.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }
            //----------------------------------
            // 大運 →＊
            //----------------------------------
            //大運（干） - 日（干）
            kyoki = tblMng.kangouTbl.GetKyoki(kansiTaiun.kan, nikkansi.kan);
            if (kyoki != null)
            {
                kansiTaiun.kan = kyoki[0];
                nikkansi.kan = kyoki[1];
            }
            //大運（干） - 月（干）
            kyoki = tblMng.kangouTbl.GetKyoki(kansiTaiun.kan, gekkansi.kan);
            if (kyoki != null)
            {
                kansiTaiun.kan = kyoki[0];
                gekkansi.kan = kyoki[1];
            }
            //大運（干） - 年（干）
            kyoki = tblMng.kangouTbl.GetKyoki(kansiTaiun.kan, nenkansi.kan);
            if (kyoki != null)
            {
                kansiTaiun.kan = kyoki[0];
                nenkansi.kan = kyoki[1];
            }

        }
    }
}
