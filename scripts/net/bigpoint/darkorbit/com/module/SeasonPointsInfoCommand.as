package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SeasonPointsInfoCommand implements IModule
   {
      private static var _instance:SeasonPointsInfoCommand;
      
      public var points:int = 0;
      
      public function SeasonPointsInfoCommand(param1:int = 0)
      {
         super();
         this.points = param1;
      }
      
      public static function get instance() : SeasonPointsInfoCommand
      {
         return _instance || (_instance = new SeasonPointsInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -26413;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.points = param1.readInt();
         this.points = this.points << 6 | this.points >>> 26;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-26413);
         param1.writeInt(this.points >>> 6 | this.points << 26);
      }
   }
}

