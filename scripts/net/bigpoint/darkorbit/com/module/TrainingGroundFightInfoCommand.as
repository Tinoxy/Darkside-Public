package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TrainingGroundFightInfoCommand implements IModule
   {
      private static var _instance:TrainingGroundFightInfoCommand;
      
      public var opponentHitpointsMax:int = 0;
      
      public var playerVictoryPoint:int = 0;
      
      public var playerHitpointsMax:int = 0;
      
      public var playerHitpoints:int = 0;
      
      public var playerShield:int = 0;
      
      public var playerShieldMax:int = 0;
      
      public var opponentVictoryPoint:int = 0;
      
      public var opponentHitpoints:int = 0;
      
      public var opponentShieldMax:int = 0;
      
      public var opponentShield:int = 0;
      
      public function TrainingGroundFightInfoCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0, param7:int = 0, param8:int = 0, param9:int = 0, param10:int = 0)
      {
         super();
         this.opponentHitpointsMax = param2;
         this.playerVictoryPoint = param10;
         this.playerHitpointsMax = param7;
         this.playerHitpoints = param6;
         this.playerShield = param8;
         this.playerShieldMax = param9;
         this.opponentVictoryPoint = param5;
         this.opponentHitpoints = param1;
         this.opponentShieldMax = param4;
         this.opponentShield = param3;
      }
      
      public static function get instance() : TrainingGroundFightInfoCommand
      {
         return _instance || (_instance = new TrainingGroundFightInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -5385;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 40;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.opponentHitpointsMax = param1.readInt();
         this.opponentHitpointsMax = this.opponentHitpointsMax >>> 13 | this.opponentHitpointsMax << 19;
         this.playerVictoryPoint = param1.readInt();
         this.playerVictoryPoint = this.playerVictoryPoint << 9 | this.playerVictoryPoint >>> 23;
         this.playerHitpointsMax = param1.readInt();
         this.playerHitpointsMax = this.playerHitpointsMax >>> 10 | this.playerHitpointsMax << 22;
         this.playerHitpoints = param1.readInt();
         this.playerHitpoints = this.playerHitpoints >>> 13 | this.playerHitpoints << 19;
         this.playerShield = param1.readInt();
         this.playerShield = this.playerShield << 16 | this.playerShield >>> 16;
         this.playerShieldMax = param1.readInt();
         this.playerShieldMax = this.playerShieldMax >>> 4 | this.playerShieldMax << 28;
         this.opponentVictoryPoint = param1.readInt();
         this.opponentVictoryPoint = this.opponentVictoryPoint << 7 | this.opponentVictoryPoint >>> 25;
         this.opponentHitpoints = param1.readInt();
         this.opponentHitpoints = this.opponentHitpoints << 1 | this.opponentHitpoints >>> 31;
         this.opponentShieldMax = param1.readInt();
         this.opponentShieldMax = this.opponentShieldMax >>> 11 | this.opponentShieldMax << 21;
         this.opponentShield = param1.readInt();
         this.opponentShield = this.opponentShield >>> 12 | this.opponentShield << 20;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-5385);
         param1.writeInt(this.opponentHitpointsMax << 13 | this.opponentHitpointsMax >>> 19);
         param1.writeInt(this.playerVictoryPoint >>> 9 | this.playerVictoryPoint << 23);
         param1.writeInt(this.playerHitpointsMax << 10 | this.playerHitpointsMax >>> 22);
         param1.writeInt(this.playerHitpoints << 13 | this.playerHitpoints >>> 19);
         param1.writeInt(this.playerShield >>> 16 | this.playerShield << 16);
         param1.writeInt(this.playerShieldMax << 4 | this.playerShieldMax >>> 28);
         param1.writeInt(this.opponentVictoryPoint >>> 7 | this.opponentVictoryPoint << 25);
         param1.writeInt(this.opponentHitpoints >>> 1 | this.opponentHitpoints << 31);
         param1.writeInt(this.opponentShieldMax << 11 | this.opponentShieldMax >>> 21);
         param1.writeInt(this.opponentShield << 12 | this.opponentShield >>> 20);
      }
   }
}

