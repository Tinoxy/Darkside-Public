package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ModuleStatusUpdateCommand implements IModule
   {
      private static var _instance:ModuleStatusUpdateCommand;
      
      public var asteroidId:int = 0;
      
      public var slotId:int = 0;
      
      public var hitpoints:int = 0;
      
      public var hitpointsMax:int = 0;
      
      public var shield:int = 0;
      
      public var shieldMax:int = 0;
      
      public var emergencyRepairSecondsLeft:int = 0;
      
      public var emergencyRepairSecondsTotal:int = 0;
      
      public function ModuleStatusUpdateCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0, param7:int = 0, param8:int = 0)
      {
         super();
         this.asteroidId = param1;
         this.slotId = param2;
         this.hitpoints = param3;
         this.hitpointsMax = param4;
         this.shield = param5;
         this.shieldMax = param6;
         this.emergencyRepairSecondsLeft = param7;
         this.emergencyRepairSecondsTotal = param8;
      }
      
      public static function get instance() : ModuleStatusUpdateCommand
      {
         return _instance || (_instance = new ModuleStatusUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 7002;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 32;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.asteroidId = param1.readInt();
         this.asteroidId = this.asteroidId << 8 | this.asteroidId >>> 24;
         this.slotId = param1.readInt();
         this.slotId = this.slotId << 2 | this.slotId >>> 30;
         this.hitpoints = param1.readInt();
         this.hitpoints = this.hitpoints >>> 1 | this.hitpoints << 31;
         this.hitpointsMax = param1.readInt();
         this.hitpointsMax = this.hitpointsMax << 12 | this.hitpointsMax >>> 20;
         this.shield = param1.readInt();
         this.shield = this.shield << 7 | this.shield >>> 25;
         this.shieldMax = param1.readInt();
         this.shieldMax = this.shieldMax >>> 14 | this.shieldMax << 18;
         this.emergencyRepairSecondsLeft = param1.readInt();
         this.emergencyRepairSecondsLeft = this.emergencyRepairSecondsLeft >>> 13 | this.emergencyRepairSecondsLeft << 19;
         this.emergencyRepairSecondsTotal = param1.readInt();
         this.emergencyRepairSecondsTotal = this.emergencyRepairSecondsTotal << 4 | this.emergencyRepairSecondsTotal >>> 28;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(7002);
         param1.writeInt(this.asteroidId >>> 8 | this.asteroidId << 24);
         param1.writeInt(this.slotId >>> 2 | this.slotId << 30);
         param1.writeInt(this.hitpoints << 1 | this.hitpoints >>> 31);
         param1.writeInt(this.hitpointsMax >>> 12 | this.hitpointsMax << 20);
         param1.writeInt(this.shield >>> 7 | this.shield << 25);
         param1.writeInt(this.shieldMax << 14 | this.shieldMax >>> 18);
         param1.writeInt(this.emergencyRepairSecondsLeft << 13 | this.emergencyRepairSecondsLeft >>> 19);
         param1.writeInt(this.emergencyRepairSecondsTotal >>> 4 | this.emergencyRepairSecondsTotal << 28);
      }
   }
}

