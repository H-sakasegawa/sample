using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    /// <summary>
    /// 十二親干法データ作成クラス
    /// </summary>
    class JuniSinKanHou
    {
        Person person;
        Insen insen;

        TableMng tblMng = TableMng.GetTblManage();
        /// <summary>
        /// personを起点として十二親干法のツリー情報を生成
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Node Create(Person _person)
        {
            person = _person;
            insen = new Insen(person);

            Node nodeBase = new Node(person.nikkansi.kan, person.gender, 0, Const.enumOyakoID.Myself);

            //親方向の探索
            CreateParentNodes(nodeBase, 2); //２親等まで

            //夫婦関係
            CreatePartnerNodes(nodeBase, 1);//１親等まで
            if (nodeBase.gender == Gender.WOMAN)
            {
                CreateChildNodes(nodeBase, 1);//１親等まで
            }
            return nodeBase;
        }

        public int CreateParentNodes(Node node, int maxKinshipNo)
        {
            Const.enumOyakoID oyako = Const.enumOyakoID.None;
            if (node.kinshipNo == 0)
            {
                oyako = Const.enumOyakoID.Mother;
            }
            //親との関係は、干合の生じさせる項目の陰陽の関係
            string parentKan = tblMng.jyukanTbl.GetOccursKangouStrAndOtherInyou(node.kan);

            parentKan = RefrectKan(parentKan, oyako);

            Node parent = new Node(parentKan, Gender.WOMAN, node.kinshipNo+1, oyako);
            parent.child = node;

            node.parent = parent;

            //夫婦関係
            CreatePartnerNodes(parent, maxKinshipNo);

            if (parent.kinshipNo < maxKinshipNo)
            {
                //親
                CreateParentNodes(parent, maxKinshipNo);
            }
            return 0;
        }
        public int CreatePartnerNodes(Node node, int maxKinshipNo)
        {
            Const.enumOyakoID oyako = Const.enumOyakoID.None;
            if (node.kinshipNo==0)
            {
                oyako = Const.enumOyakoID.Spouse; //配偶者
            }
            else
            {
                if (node.kinshipNo == 1)
                {
                    oyako = (node.gender == Gender.WOMAN ? Const.enumOyakoID.Father : Const.enumOyakoID.Mother);
                }
            }

            //夫婦関係は、干合の関係
            string partnerKan = tblMng.kangouTbl.GetKangouOtherStr(node.kan);
            partnerKan = RefrectKan(partnerKan, oyako);


            Node partner = new Node(partnerKan,
                                    node.gender == Gender.WOMAN ? Gender.MAN : Gender.WOMAN,
                                    node.kinshipNo+1,
                                    oyako
                                    );
            if (node.gender == Gender.WOMAN)
            {
                //partner.partnerWoman = node;
                node.partnerMan = partner;
            }
            else
            {
                //partner.partnerMan = node;
                node.partnerWoman = partner;
            }


            if (node.kinshipNo < maxKinshipNo)
            {
                //親のノードを作成
                CreateParentNodes(partner, maxKinshipNo);
                if (partner.gender == Gender.WOMAN)
                {
                    //子のノードを作成
                    CreateChildNodes(partner, maxKinshipNo);
                }
            }            
            return 0;
        }
        public int CreateChildNodes(Node node, int maxKinshipNo)
        {
            Const.enumOyakoID oyako = Const.enumOyakoID.None;
            if (node.kinshipNo <=1)
            {
                oyako = Const.enumOyakoID.Child;
            }

            //親との関係は、干合の生じさせる項目の陰陽の関係

            string childKan = tblMng.jyukanTbl.GetCauseKangouStrAndOtherInyou(node.kan);
            childKan = RefrectKan(childKan, oyako);

            //とりあえず男の子として作成
            Node childNode = new Node(childKan, Gender.MAN, node.kinshipNo + 1, oyako);
            node.child = childNode;

           // node.parent = parent;

            //夫婦関係
            CreatePartnerNodes(childNode, maxKinshipNo);

            //if (parent.kinshipNo < maxKinshipNo)
            //{
            //    //親
            //    CreateChildNodes(parent, maxKinshipNo);
            //}
            return 0;
        }

        string RefrectKan(string kan, Const.enumOyakoID oyako)
        {
            string result = kan;

            if (!string.IsNullOrEmpty(result))
            {

                //検索された干が蔵元に含まれているか？
                if (!insen.IsExist(kan))
                {
                    //含まれていなければ、十干テーブルから陰陽の関係にある干を取得
                    result = tblMng.jyukanTbl.GetInyouOtherString(kan);

                    //陰陽の関係から取得した干が陰占、蔵元に含まれているか？
                    if (!insen.IsExist(result))
                    {
                        //場所から取得
                        //[自分]    [子供] [ 父 ]
                        //[配偶者]  [ -- ] [ 母 ] 
                        switch (oyako)
                        {
                            case Const.enumOyakoID.Child:
                                result = insen.gekkansi.kan;
                                break;
                            case Const.enumOyakoID.Father:
                                result = insen.nenkansi.kan;
                                break;
                            case Const.enumOyakoID.Spouse:
                                result = insen.nikkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN].name;
                                break;
                            case Const.enumOyakoID.Mother:
                                result = insen.nenkansiHongen[(int)NijuhachiGenso.enmGensoType.GENSO_HONGEN].name;
                                break;
                            default:
                                result = "";
                                break;
                        }
                        if (string.IsNullOrEmpty(result)) result = null;
                    }
                }
            }
            return result;
        }
    }

    class Node
    {
        public Node( string _kan, Gender _gender, int _kinshipNo, Const.enumOyakoID oyako)
        {
            kan = _kan;
            gender = _gender;
            kinshipNo = _kinshipNo;
            enmOyako = oyako;
        }
        public Gender gender;
        public string kan="";

        public int kinshipNo=0; //  n親等

        public Node partnerMan = null;
        public Node partnerWoman = null;
        public Node parent = null;
        public Node child = null;

        public Const.enumOyakoID enmOyako;

    }
}
