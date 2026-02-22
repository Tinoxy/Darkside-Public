package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class RogueLiteGateRewardModule implements IModule
   {
      private static var _instance:RogueLiteGateRewardModule;
      
      public var amount:int = 0;
      
      public var lootId:String = "";
      
      public var level:int = 0;
      
      public function RogueLiteGateRewardModule(param1:String = "", param2:int = 0, param3:int = 0)
      {
         super();
         this.amount = param3;
         this.lootId = param1;
         this.level = param2;
      }
      
      public static function get instance() : RogueLiteGateRewardModule
      {
         return _instance || (_instance = new RogueLiteGateRewardModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 16248;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.amount = param1.readInt();
         this.amount = this.amount << 9 | this.amount >>> 23;
         this.lootId = param1.readUTF();
         this.level = param1.readInt();
         this.level = this.level << 7 | this.level >>> 25;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(16248);
         param1.writeInt(this.amount >>> 9 | this.amount << 23);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.level >>> 7 | this.level << 25);
      }
   }
}

