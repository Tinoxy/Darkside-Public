package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class BattlePassKeyExchangeRewardModule implements IModule
   {
      private static var _instance:BattlePassKeyExchangeRewardModule;
      
      public var exchangeReward:String = "";
      
      public var cost:int = 0;
      
      public var isSpecial:Boolean = false;
      
      public var listOfloot:Vector.<BattlePassKeyExchangeLootModule>;
      
      public var limit:int = 0;
      
      public var priority:int = 0;
      
      public var claimCount:int = 0;
      
      public function BattlePassKeyExchangeRewardModule(param1:String = "", param2:Vector.<BattlePassKeyExchangeLootModule> = null, param3:int = 0, param4:int = 0, param5:int = 0, param6:Boolean = false, param7:int = 0)
      {
         super();
         this.exchangeReward = param1;
         this.cost = param3;
         this.isSpecial = param6;
         if(param2 == null)
         {
            this.listOfloot = new Vector.<BattlePassKeyExchangeLootModule>();
         }
         else
         {
            this.listOfloot = param2;
         }
         this.limit = param4;
         this.priority = param5;
         this.claimCount = param7;
      }
      
      public static function get instance() : BattlePassKeyExchangeRewardModule
      {
         return _instance || (_instance = new BattlePassKeyExchangeRewardModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 1610;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 23;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:BattlePassKeyExchangeLootModule = null;
         this.exchangeReward = param1.readUTF();
         this.cost = param1.readInt();
         this.cost = this.cost >>> 8 | this.cost << 24;
         this.isSpecial = param1.readBoolean();
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.listOfloot.length > 0)
         {
            this.listOfloot.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = BattlePassKeyExchangeLootModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.listOfloot.push(_loc4_);
            _loc2_++;
         }
         this.limit = param1.readInt();
         this.limit = this.limit >>> 6 | this.limit << 26;
         this.priority = param1.readInt();
         this.priority = this.priority << 13 | this.priority >>> 19;
         this.claimCount = param1.readInt();
         this.claimCount = this.claimCount << 6 | this.claimCount >>> 26;
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:BattlePassKeyExchangeLootModule = null;
         param1.writeShort(1610);
         param1.writeUTF(this.exchangeReward);
         param1.writeInt(this.cost << 8 | this.cost >>> 24);
         param1.writeBoolean(this.isSpecial);
         param1.writeByte(this.listOfloot.length);
         for each(_loc2_ in this.listOfloot)
         {
            _loc2_.write(param1);
         }
         param1.writeInt(this.limit << 6 | this.limit >>> 26);
         param1.writeInt(this.priority >>> 13 | this.priority << 19);
         param1.writeInt(this.claimCount >>> 6 | this.claimCount << 26);
      }
   }
}

