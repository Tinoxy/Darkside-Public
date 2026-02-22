package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderBonusGGRewardModule implements IModule
   {
      private static var _instance:GalaxyGateBuilderBonusGGRewardModule;
      
      public var amount:int = 0;
      
      public var lootId:String = "";
      
      public var countdown:int = 0;
      
      public var claimed:Boolean = false;
      
      public function GalaxyGateBuilderBonusGGRewardModule(param1:String = "", param2:int = 0, param3:Boolean = false, param4:int = 0)
      {
         super();
         this.amount = param2;
         this.lootId = param1;
         this.countdown = param4;
         this.claimed = param3;
      }
      
      public static function get instance() : GalaxyGateBuilderBonusGGRewardModule
      {
         return _instance || (_instance = new GalaxyGateBuilderBonusGGRewardModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -16507;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 11;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.amount = param1.readInt();
         this.amount = this.amount << 8 | this.amount >>> 24;
         this.lootId = param1.readUTF();
         this.countdown = param1.readInt();
         this.countdown = this.countdown >>> 8 | this.countdown << 24;
         this.claimed = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-16507);
         param1.writeInt(this.amount >>> 8 | this.amount << 24);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.countdown << 8 | this.countdown >>> 24);
         param1.writeBoolean(this.claimed);
      }
   }
}

