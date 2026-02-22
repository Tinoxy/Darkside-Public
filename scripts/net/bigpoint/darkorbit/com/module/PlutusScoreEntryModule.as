package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PlutusScoreEntryModule implements IModule
   {
      private static var _instance:PlutusScoreEntryModule;
      
      public var timeInSeconds:int = 0;
      
      public var score:int = 0;
      
      public function PlutusScoreEntryModule(param1:int = 0, param2:int = 0)
      {
         super();
         this.timeInSeconds = param2;
         this.score = param1;
      }
      
      public static function get instance() : PlutusScoreEntryModule
      {
         return _instance || (_instance = new PlutusScoreEntryModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 32247;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.timeInSeconds = param1.readInt();
         this.timeInSeconds = this.timeInSeconds << 4 | this.timeInSeconds >>> 28;
         this.score = param1.readInt();
         this.score = this.score >>> 10 | this.score << 22;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(32247);
         param1.writeInt(this.timeInSeconds >>> 4 | this.timeInSeconds << 28);
         param1.writeInt(this.score << 10 | this.score >>> 22);
      }
   }
}

