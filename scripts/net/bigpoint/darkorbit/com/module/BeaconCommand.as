package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class BeaconCommand implements IModule
   {
      private static var _instance:BeaconCommand;
      
      public var inPeaceArea:Boolean = false;
      
      public var positionAheadY:int = 0;
      
      public var isAlarmEffectActive:Boolean = false;
      
      public var positionY:int = 0;
      
      public var repairRobotActive:Boolean = false;
      
      public var repairRobotLootId:String = "";
      
      public var positionX:int = 0;
      
      public var inStation:Boolean = false;
      
      public var nearJumpGate:Boolean = false;
      
      public var positionAheadX:int = 0;
      
      public var inRadiationZone:Boolean = false;
      
      public var repairBotSkilled:Boolean = false;
      
      public function BeaconCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:Boolean = false, param6:Boolean = false, param7:Boolean = false, param8:Boolean = false, param9:Boolean = false, param10:String = "", param11:Boolean = false, param12:Boolean = false)
      {
         super();
         this.inPeaceArea = param5;
         this.positionAheadY = param4;
         this.isAlarmEffectActive = param12;
         this.positionY = param2;
         this.repairRobotActive = param8;
         this.repairRobotLootId = param10;
         this.positionX = param1;
         this.inStation = param7;
         this.nearJumpGate = param6;
         this.positionAheadX = param3;
         this.inRadiationZone = param11;
         this.repairBotSkilled = param9;
      }
      
      public static function get instance() : BeaconCommand
      {
         return _instance || (_instance = new BeaconCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 4853;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 25;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.inPeaceArea = param1.readBoolean();
         this.positionAheadY = param1.readInt();
         this.positionAheadY = this.positionAheadY << 12 | this.positionAheadY >>> 20;
         this.isAlarmEffectActive = param1.readBoolean();
         this.positionY = param1.readInt();
         this.positionY = this.positionY << 11 | this.positionY >>> 21;
         this.repairRobotActive = param1.readBoolean();
         this.repairRobotLootId = param1.readUTF();
         this.positionX = param1.readInt();
         this.positionX = this.positionX << 1 | this.positionX >>> 31;
         this.inStation = param1.readBoolean();
         this.nearJumpGate = param1.readBoolean();
         this.positionAheadX = param1.readInt();
         this.positionAheadX = this.positionAheadX >>> 14 | this.positionAheadX << 18;
         this.inRadiationZone = param1.readBoolean();
         this.repairBotSkilled = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(4853);
         param1.writeBoolean(this.inPeaceArea);
         param1.writeInt(this.positionAheadY >>> 12 | this.positionAheadY << 20);
         param1.writeBoolean(this.isAlarmEffectActive);
         param1.writeInt(this.positionY >>> 11 | this.positionY << 21);
         param1.writeBoolean(this.repairRobotActive);
         param1.writeUTF(this.repairRobotLootId);
         param1.writeInt(this.positionX >>> 1 | this.positionX << 31);
         param1.writeBoolean(this.inStation);
         param1.writeBoolean(this.nearJumpGate);
         param1.writeInt(this.positionAheadX << 14 | this.positionAheadX >>> 18);
         param1.writeBoolean(this.inRadiationZone);
         param1.writeBoolean(this.repairBotSkilled);
      }
   }
}

