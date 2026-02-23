package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ExtraInfoCommand implements IModule
   {
      private static var _instance:ExtraInfoCommand;
      
      public var droneCPU:int = 0;
      
      public var diploCPU:int = 0;
      
      public var jumpCPU:int = 0;
      
      public var ammoCPU:int = 0;
      
      public var smartBomb:int = 0;
      
      public var instaShield:int = 0;
      
      public var mineTurbo:int = 0;
      
      public var aimCPU:int = 0;
      
      public var arolCPU:int = 0;
      
      public var cloakCPU:int = 0;
      
      public var autoRlCPU:int = 0;
      
      public var rocketBuyCPU:int = 0;
      
      public var specialJumpCPU:int = 0;
      
      public function ExtraInfoCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0, param7:int = 0, param8:int = 0, param9:int = 0, param10:int = 0, param11:int = 0, param12:int = 0, param13:int = 0)
      {
         super();
         this.droneCPU = param1;
         this.diploCPU = param2;
         this.jumpCPU = param3;
         this.ammoCPU = param4;
         this.smartBomb = param5;
         this.instaShield = param6;
         this.mineTurbo = param7;
         this.aimCPU = param8;
         this.arolCPU = param9;
         this.cloakCPU = param10;
         this.autoRlCPU = param11;
         this.rocketBuyCPU = param12;
         this.specialJumpCPU = param13;
      }
      
      public static function get instance() : ExtraInfoCommand
      {
         return _instance || (_instance = new ExtraInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 56;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 52;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.droneCPU = param1.readInt();
         this.droneCPU = this.droneCPU << 13 | this.droneCPU >>> 19;
         this.diploCPU = param1.readInt();
         this.diploCPU = this.diploCPU << 11 | this.diploCPU >>> 21;
         this.jumpCPU = param1.readInt();
         this.jumpCPU = this.jumpCPU << 6 | this.jumpCPU >>> 26;
         this.ammoCPU = param1.readInt();
         this.ammoCPU = this.ammoCPU >>> 11 | this.ammoCPU << 21;
         this.smartBomb = param1.readInt();
         this.smartBomb = this.smartBomb >>> 10 | this.smartBomb << 22;
         this.instaShield = param1.readInt();
         this.instaShield = this.instaShield << 2 | this.instaShield >>> 30;
         this.mineTurbo = param1.readInt();
         this.mineTurbo = this.mineTurbo << 16 | this.mineTurbo >>> 16;
         this.aimCPU = param1.readInt();
         this.aimCPU = this.aimCPU << 3 | this.aimCPU >>> 29;
         this.arolCPU = param1.readInt();
         this.arolCPU = this.arolCPU >>> 8 | this.arolCPU << 24;
         this.cloakCPU = param1.readInt();
         this.cloakCPU = this.cloakCPU << 11 | this.cloakCPU >>> 21;
         this.autoRlCPU = param1.readInt();
         this.autoRlCPU = this.autoRlCPU << 16 | this.autoRlCPU >>> 16;
         this.rocketBuyCPU = param1.readInt();
         this.rocketBuyCPU = this.rocketBuyCPU >>> 1 | this.rocketBuyCPU << 31;
         this.specialJumpCPU = param1.readInt();
         this.specialJumpCPU = this.specialJumpCPU << 4 | this.specialJumpCPU >>> 28;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(56);
         param1.writeInt(this.droneCPU >>> 13 | this.droneCPU << 19);
         param1.writeInt(this.diploCPU >>> 11 | this.diploCPU << 21);
         param1.writeInt(this.jumpCPU >>> 6 | this.jumpCPU << 26);
         param1.writeInt(this.ammoCPU << 11 | this.ammoCPU >>> 21);
         param1.writeInt(this.smartBomb << 10 | this.smartBomb >>> 22);
         param1.writeInt(this.instaShield >>> 2 | this.instaShield << 30);
         param1.writeInt(this.mineTurbo >>> 16 | this.mineTurbo << 16);
         param1.writeInt(this.aimCPU >>> 3 | this.aimCPU << 29);
         param1.writeInt(this.arolCPU << 8 | this.arolCPU >>> 24);
         param1.writeInt(this.cloakCPU >>> 11 | this.cloakCPU << 21);
         param1.writeInt(this.autoRlCPU >>> 16 | this.autoRlCPU << 16);
         param1.writeInt(this.rocketBuyCPU << 1 | this.rocketBuyCPU >>> 31);
         param1.writeInt(this.specialJumpCPU >>> 4 | this.specialJumpCPU << 28);
      }
   }
}

