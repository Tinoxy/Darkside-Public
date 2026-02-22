package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TrainingArenaRankInfoCommand implements IModule
   {
      private static var _instance:TrainingArenaRankInfoCommand;
      
      public var points:Number = 0;
      
      public var rank:int = 0;
      
      public var victories:int = 0;
      
      public var battles:int = 0;
      
      public function TrainingArenaRankInfoCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:Number = 0)
      {
         super();
         this.points = param4;
         this.rank = param3;
         this.victories = param1;
         this.battles = param2;
      }
      
      public static function get instance() : TrainingArenaRankInfoCommand
      {
         return _instance || (_instance = new TrainingArenaRankInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -3982;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.points = param1.readFloat();
         this.rank = param1.readInt();
         this.rank = this.rank >>> 12 | this.rank << 20;
         this.victories = param1.readInt();
         this.victories = this.victories << 12 | this.victories >>> 20;
         this.battles = param1.readInt();
         this.battles = this.battles << 11 | this.battles >>> 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-3982);
         param1.writeFloat(this.points);
         param1.writeInt(this.rank << 12 | this.rank >>> 20);
         param1.writeInt(this.victories >>> 12 | this.victories << 20);
         param1.writeInt(this.battles >>> 11 | this.battles << 21);
      }
   }
}

