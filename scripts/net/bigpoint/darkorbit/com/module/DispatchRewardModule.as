package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class DispatchRewardModule extends RewardContentModule
   {
      private static var _instance:DispatchRewardModule;
      
      public var amount:int = 0;
      
      public var lootId:String = "";
      
      public var chance:int = 0;
      
      public function DispatchRewardModule(param1:String = "", param2:int = 0, param3:int = 0)
      {
         super();
         this.amount = param2;
         this.lootId = param1;
         this.chance = param3;
      }
      
      public static function get instance() : DispatchRewardModule
      {
         return _instance || (_instance = new DispatchRewardModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 19909;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.amount = param1.readInt();
         this.amount = this.amount << 11 | this.amount >>> 21;
         this.lootId = param1.readUTF();
         this.chance = param1.readShort();
         this.chance = 65535 & ((65535 & this.chance) >>> 9 | (65535 & this.chance) << 7);
         this.chance = this.chance > 32767 ? int(this.chance - 65536) : this.chance;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(19909);
         super.write(param1);
         param1.writeInt(this.amount >>> 11 | this.amount << 21);
         param1.writeUTF(this.lootId);
         param1.writeShort(65535 & ((65535 & this.chance) << 9 | (65535 & this.chance) >>> 7));
      }
   }
}

