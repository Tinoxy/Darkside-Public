package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TeamDeathMatchResultScreenPlayerModule implements IModule
   {
      private static var _instance:TeamDeathMatchResultScreenPlayerModule;
      
      public var kills:int = 0;
      
      public var deaths:int = 0;
      
      public var pointsDelta:int = 0;
      
      public var totalDamageGiven:Number = 0;
      
      public var totalDamageTaken:Number = 0;
      
      public var peakDamageGiven:int = 0;
      
      public var winningStreak:int = 0;
      
      public var name:String = "";
      
      public function TeamDeathMatchResultScreenPlayerModule(param1:int = 0, param2:int = 0, param3:int = 0, param4:Number = 0, param5:Number = 0, param6:int = 0, param7:int = 0, param8:String = "")
      {
         super();
         this.kills = param1;
         this.deaths = param2;
         this.pointsDelta = param3;
         this.totalDamageGiven = param4;
         this.totalDamageTaken = param5;
         this.peakDamageGiven = param6;
         this.winningStreak = param7;
         this.name = param8;
      }
      
      public static function get instance() : TeamDeathMatchResultScreenPlayerModule
      {
         return _instance || (_instance = new TeamDeathMatchResultScreenPlayerModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 1724;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 38;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.kills = param1.readInt();
         this.kills = this.kills << 1 | this.kills >>> 31;
         this.deaths = param1.readInt();
         this.deaths = this.deaths >>> 11 | this.deaths << 21;
         this.pointsDelta = param1.readInt();
         this.pointsDelta = this.pointsDelta >>> 14 | this.pointsDelta << 18;
         this.totalDamageGiven = param1.readDouble();
         this.totalDamageTaken = param1.readDouble();
         this.peakDamageGiven = param1.readInt();
         this.peakDamageGiven = this.peakDamageGiven >>> 9 | this.peakDamageGiven << 23;
         this.winningStreak = param1.readInt();
         this.winningStreak = this.winningStreak << 1 | this.winningStreak >>> 31;
         this.name = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(1724);
         param1.writeInt(this.kills >>> 1 | this.kills << 31);
         param1.writeInt(this.deaths << 11 | this.deaths >>> 21);
         param1.writeInt(this.pointsDelta << 14 | this.pointsDelta >>> 18);
         param1.writeDouble(this.totalDamageGiven);
         param1.writeDouble(this.totalDamageTaken);
         param1.writeInt(this.peakDamageGiven << 9 | this.peakDamageGiven >>> 23);
         param1.writeInt(this.winningStreak >>> 1 | this.winningStreak << 31);
         param1.writeUTF(this.name);
      }
   }
}

