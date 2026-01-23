namespace DarksideApi.DarkOrbit.Commands.Shop
{
    public class ShopItemCommand : Command
    {
        public new const short Id = -30521;
        public long saleRemainingTime;
        public int maxlevel;
        public bool buyMore;
        public bool isSubscriptionRunning;
        public string lootId = string.Empty;
        public string cost = string.Empty;
        public string levelGrades = string.Empty;
        public int maxAmount;
        public string discountPercent = string.Empty;
        public bool canBeSubscribedTo;
        public string category = string.Empty;
        public bool isSubscriptionPaused;
        public string amountsSelectable = string.Empty;
        public List<ShopItemLevelDataCommand> LevelDataList = [];
        public bool userHasBoosterPackage;
        public int isPaymentItem;
        public int elite;
        public int isActiveEventItem;
        public long secondsLeft;
        public string currency = string.Empty;
        public int isServerExclusive;
        public List<float> price = [];
        public LimitType limitType;
        public int limitMax;
        public int expiresSeconds;
        public int limitAmount;

        public override void Read(ByteArray reader)
        {
            this.maxlevel = reader.ReadInt();
            this.maxlevel = this.maxlevel >>> 19 | this.maxlevel << 13;

            this.currency = reader.ReadUTF();
            this.canBeSubscribedTo = reader.ReadBool();
            this.limitType = (LimitType)reader.ReadShort();
            this.buyMore = reader.ReadBool();
            this.isPaymentItem = reader.ReadInt();
            this.isPaymentItem = this.isPaymentItem >>> 25 | this.isPaymentItem << 7;
            
            this.saleRemainingTime = (long)reader.ReadDouble();
            this.limitMax = reader.ReadInt();
            this.limitMax = this.limitMax >>> 19 | this.limitMax << 13;
            this.isServerExclusive = reader.ReadInt();
            this.isServerExclusive = this.isServerExclusive << 14 | this.isServerExclusive >>> 18;
            this.levelGrades = reader.ReadUTF();
            this.discountPercent = reader.ReadUTF();
            this.isSubscriptionPaused = reader.ReadBool();
            this.isSubscriptionRunning = reader.ReadBool();
            this.amountsSelectable = reader.ReadUTF();
            this.isActiveEventItem = reader.ReadInt();
            this.isActiveEventItem = this.isActiveEventItem << 17 | this.isActiveEventItem >>> 15;
            this.secondsLeft = (long)reader.ReadDouble();
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                this.price.Add(reader.ReadFloat());
                i++;
            }
            this.expiresSeconds = reader.ReadInt();
            this.expiresSeconds = this.expiresSeconds << 17 | this.expiresSeconds >>> 15;
            this.elite = reader.ReadInt();
            this.elite = this.elite >>> 25 | this.elite << 7;
            this.maxAmount = reader.ReadInt();
            this.maxAmount = this.maxAmount >>> 25 | this.maxAmount << 7;
            this.category = reader.ReadUTF();
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var e = new ShopItemLevelDataCommand();
                e.Read(reader);
                this.LevelDataList.Add(e);
                i++;
            }
            this.cost = reader.ReadUTF();
            this.userHasBoosterPackage = reader.ReadBool();
            this.lootId = reader.ReadUTF();
            this.limitAmount = reader.ReadInt();
            this.limitAmount = this.limitAmount >>> 25 | this.limitAmount << 7;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum LimitType
        {
            none,
            daily,
            weekly,
            monthly,
        }

        public override string ToString()
        {
            var priceString = string.Join(", ", this.price.Select(p => p.ToString("F2")));
            var levelDataListString = string.Join(", ", this.LevelDataList.Select(data => data.ToString()));
            return $"ShopItemCommand{{" +
            $"saleRemainingTime={this.saleRemainingTime}, " +
            $"maxlevel={this.maxlevel}, " +
            $"buyMore={this.buyMore}, " +
            $"isSubscriptionRunning={this.isSubscriptionRunning}, " +
            $"lootId='{this.lootId}', " +
            $"cost='{this.cost}', " +
            $"levelGrades='{this.levelGrades}', " +
            $"maxAmount={this.maxAmount}, " +
            $"discountPercent='{this.discountPercent}', " +
            $"canBeSubscribedTo={this.canBeSubscribedTo}, " +
            $"category='{this.category}', " +
            $"isSubscriptionPaused={this.isSubscriptionPaused}, " +
            $"amountsSelectable='{this.amountsSelectable}', " +
            $"LevelDataList={levelDataListString}, " +
            $"userHasBoosterPackage={this.userHasBoosterPackage}, " +
            $"isPaymentItem={this.isPaymentItem}, " +
            $"elite={this.elite}, " +
            $"isActiveEventItem={this.isActiveEventItem}, " +
            $"secondsLeft={this.secondsLeft}, " +
            $"currency='{this.currency}', " +
            $"isServerExclusive={this.isServerExclusive}, " +
            $"price={priceString}, " +
            $"limitType={this.limitType}, " +
            $"limitMax={this.limitMax}, " +
            $"expiresSeconds={this.expiresSeconds}, " +
            $"limitAmount={this.limitAmount}\n";
        }
    }
}
