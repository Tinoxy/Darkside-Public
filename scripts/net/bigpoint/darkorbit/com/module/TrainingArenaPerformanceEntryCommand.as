package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TrainingArenaPerformanceEntryCommand implements IModule
   {
      private static var _instance:TrainingArenaPerformanceEntryCommand;
      
      public var rank:int = 0;
      
      public var seasonName:String = "";
      
      public function TrainingArenaPerformanceEntryCommand(param1:String = "", param2:int = 0)
      {
         super();
         this.rank = param2;
         this.seasonName = param1;
      }
      
      public static function get instance() : TrainingArenaPerformanceEntryCommand
      {
         return _instance || (_instance = new TrainingArenaPerformanceEntryCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 2019;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.rank = param1.readInt();
         this.rank = this.rank >>> 3 | this.rank << 29;
         this.seasonName = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(2019);
         param1.writeInt(this.rank << 3 | this.rank >>> 29);
         param1.writeUTF(this.seasonName);
      }
   }
}

