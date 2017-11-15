﻿namespace KuaiCard.Model.Payment
{
    using System;

    public class PayRate
    {
        private int _id;
        private string _levname;
        private decimal _p100;
        private decimal _p101;
        private decimal _p102;
        private decimal _p1020 = 0M;
        private decimal _p103;
        private decimal _p104;
        private decimal _p105;
        private decimal _p106;
        private decimal _p107;
        private decimal _p108;
        private decimal _p109;
        private decimal _p110;
        private decimal _p111;
        private decimal _p112;
        private decimal _p113;
        private decimal _p114;
        private decimal _p115 = 0M;
        private decimal _p116 = 0M;
        private decimal _p117 = 0M;
        private decimal _p118 = 0M;
        private decimal _p119 = 0M;
        private decimal _p200 = 0M;
        private decimal _p201 = 0M;
        private decimal _p202 = 0M;
        private decimal _p203 = 0M;
        private decimal _p204 = 0M;
        private decimal _p205 = 0M;
        private decimal _p206 = 0M;
        private decimal _p207 = 0M;
        private decimal _p208 = 0M;
        private decimal _p209 = 0M;
        private decimal _p210 = 0M;
        private decimal _p300;
        private decimal _p98;
        private decimal _p980 = 0M;
        private decimal _p99;
        private decimal _p990 = 0M;
        private decimal _p995 = 0M;
        private RateTypeEnum _ratetype;
        private int _userlevel;

        public int id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string levName
        {
            get
            {
                return this._levname;
            }
            set
            {
                this._levname = value;
            }
        }

        public decimal p100
        {
            get
            {
                return this._p100;
            }
            set
            {
                this._p100 = value;
            }
        }

        public decimal p101
        {
            get
            {
                return this._p101;
            }
            set
            {
                this._p101 = value;
            }
        }

        public decimal p102
        {
            get
            {
                return this._p102;
            }
            set
            {
                this._p102 = value;
            }
        }

        public decimal p1020
        {
            get
            {
                return this._p1020;
            }
            set
            {
                this._p1020 = value;
            }
        }

        public decimal p103
        {
            get
            {
                return this._p103;
            }
            set
            {
                this._p103 = value;
            }
        }

        public decimal p104
        {
            get
            {
                return this._p104;
            }
            set
            {
                this._p104 = value;
            }
        }

        public decimal p105
        {
            get
            {
                return this._p105;
            }
            set
            {
                this._p105 = value;
            }
        }

        public decimal p106
        {
            get
            {
                return this._p106;
            }
            set
            {
                this._p106 = value;
            }
        }

        public decimal p107
        {
            get
            {
                return this._p107;
            }
            set
            {
                this._p107 = value;
            }
        }

        public decimal p108
        {
            get
            {
                return this._p108;
            }
            set
            {
                this._p108 = value;
            }
        }

        public decimal p109
        {
            get
            {
                return this._p109;
            }
            set
            {
                this._p109 = value;
            }
        }

        public decimal p110
        {
            get
            {
                return this._p110;
            }
            set
            {
                this._p110 = value;
            }
        }

        public decimal p111
        {
            get
            {
                return this._p111;
            }
            set
            {
                this._p111 = value;
            }
        }

        public decimal p112
        {
            get
            {
                return this._p112;
            }
            set
            {
                this._p112 = value;
            }
        }

        public decimal p113
        {
            get
            {
                return this._p113;
            }
            set
            {
                this._p113 = value;
            }
        }

        public decimal p114
        {
            get
            {
                return this._p114;
            }
            set
            {
                this._p114 = value;
            }
        }

        public decimal p115
        {
            get
            {
                return this._p115;
            }
            set
            {
                this._p115 = value;
            }
        }

        public decimal p116
        {
            get
            {
                return this._p116;
            }
            set
            {
                this._p116 = value;
            }
        }

        public decimal p117
        {
            get
            {
                return this._p117;
            }
            set
            {
                this._p117 = value;
            }
        }

        public decimal p118
        {
            get
            {
                return this._p118;
            }
            set
            {
                this._p118 = value;
            }
        }

        public decimal p119
        {
            get
            {
                return this._p119;
            }
            set
            {
                this._p119 = value;
            }
        }

        public decimal p200
        {
            get
            {
                return this._p200;
            }
            set
            {
                this._p200 = value;
            }
        }

        public decimal p201
        {
            get
            {
                return this._p201;
            }
            set
            {
                this._p201 = value;
            }
        }

        public decimal p202
        {
            get
            {
                return this._p202;
            }
            set
            {
                this._p202 = value;
            }
        }

        public decimal p203
        {
            get
            {
                return this._p203;
            }
            set
            {
                this._p203 = value;
            }
        }

        public decimal p204
        {
            get
            {
                return this._p204;
            }
            set
            {
                this._p204 = value;
            }
        }

        public decimal p205
        {
            get
            {
                return this._p205;
            }
            set
            {
                this._p205 = value;
            }
        }

        public decimal p206
        {
            get
            {
                return this._p206;
            }
            set
            {
                this._p206 = value;
            }
        }

        public decimal p207
        {
            get
            {
                return this._p207;
            }
            set
            {
                this._p207 = value;
            }
        }

        public decimal p208
        {
            get
            {
                return this._p208;
            }
            set
            {
                this._p208 = value;
            }
        }

        public decimal p209
        {
            get
            {
                return this._p209;
            }
            set
            {
                this._p209 = value;
            }
        }

        public decimal p210
        {
            get
            {
                return this._p210;
            }
            set
            {
                this._p210 = value;
            }
        }

        public decimal p300
        {
            get
            {
                return this._p300;
            }
            set
            {
                this._p300 = value;
            }
        }

        public decimal p98
        {
            get
            {
                return this._p98;
            }
            set
            {
                this._p98 = value;
            }
        }

        public decimal p980
        {
            get
            {
                return this._p980;
            }
            set
            {
                this._p980 = value;
            }
        }

        public decimal p99
        {
            get
            {
                return this._p99;
            }
            set
            {
                this._p99 = value;
            }
        }

        public decimal p990
        {
            get
            {
                return this._p990;
            }
            set
            {
                this._p990 = value;
            }
        }

        public RateTypeEnum rateType
        {
            get
            {
                return this._ratetype;
            }
            set
            {
                this._ratetype = value;
            }
        }

        public int userLevel
        {
            get
            {
                return this._userlevel;
            }
            set
            {
                this._userlevel = value;
            }
        }
    }
}

