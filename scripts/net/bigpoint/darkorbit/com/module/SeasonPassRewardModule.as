package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SeasonPassRewardModule implements IModule
   {
      private static var _instance:SeasonPassRewardModule;
      
      public var goldPass:Boolean = false;
      
      public var extGoldPass:Boolean = false;
      
      public var level:int = 0;
      
      public var lootId:String = "";
      
      public var amount:int = 0;
      
      public var claimed:Boolean = false;
      
      public function SeasonPassRewardModule(param1:String = "", param2:int = 0, param3:int = 0, param4:Boolean = false, param5:Boolean = false, param6:Boolean = false)
      {
         super();
         this.goldPass = param4;
         this.extGoldPass = param6;
         this.level = param3;
         this.lootId = param1;
         this.amount = param2;
         this.claimed = param5;
      }
      
      public static function get instance() : SeasonPassRewardModule
      {
         return _instance || (_instance = new SeasonPassRewardModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 15361;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 13;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.goldPass = param1.readBoolean();
         this.extGoldPass = param1.readBoolean();
         this.level = param1.readInt();
         this.level = this.level << 4 | this.level >>> 28;
         this.lootId = param1.readUTF();
         this.amount = param1.readInt();
         this.amount = this.amount << 16 | this.amount >>> 16;
         this.claimed = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(15361);
         param1.writeBoolean(this.goldPass);
         param1.writeBoolean(this.extGoldPass);
         param1.writeInt(this.level >>> 4 | this.level << 28);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.amount >>> 16 | this.amount << 16);
         param1.writeBoolean(this.claimed);
      }
   }
}

