package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class BattlePassKeyExchangeUpdateCommand implements IModule
   {
      private static var _instance:BattlePassKeyExchangeUpdateCommand;
      
      public var lootId:String = "";
      
      public var claimCount:int = 0;
      
      public var spcId:int = 0;
      
      public var numOfKeys:int = 0;
      
      public var exchangeReward:String = "";
      
      public function BattlePassKeyExchangeUpdateCommand(param1:String = "", param2:String = "", param3:int = 0, param4:int = 0, param5:int = 0)
      {
         super();
         this.lootId = param2;
         this.claimCount = param3;
         this.spcId = param5;
         this.numOfKeys = param4;
         this.exchangeReward = param1;
      }
      
      public static function get instance() : BattlePassKeyExchangeUpdateCommand
      {
         return _instance || (_instance = new BattlePassKeyExchangeUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 29653;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.lootId = param1.readUTF();
         this.claimCount = param1.readInt();
         this.claimCount = this.claimCount << 11 | this.claimCount >>> 21;
         this.spcId = param1.readInt();
         this.spcId = this.spcId >>> 1 | this.spcId << 31;
         this.numOfKeys = param1.readInt();
         this.numOfKeys = this.numOfKeys >>> 10 | this.numOfKeys << 22;
         this.exchangeReward = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(29653);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.claimCount >>> 11 | this.claimCount << 21);
         param1.writeInt(this.spcId << 1 | this.spcId >>> 31);
         param1.writeInt(this.numOfKeys << 10 | this.numOfKeys >>> 22);
         param1.writeUTF(this.exchangeReward);
      }
   }
}

