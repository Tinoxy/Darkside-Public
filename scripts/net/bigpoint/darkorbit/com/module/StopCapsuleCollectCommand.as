package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class StopCapsuleCollectCommand implements IModule
   {
      private static var _instance:StopCapsuleCollectCommand;
      
      public var collectorId:int = 0;
      
      public function StopCapsuleCollectCommand(param1:int = 0)
      {
         super();
         this.collectorId = param1;
      }
      
      public static function get instance() : StopCapsuleCollectCommand
      {
         return _instance || (_instance = new StopCapsuleCollectCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 28990;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.collectorId = param1.readInt();
         this.collectorId = this.collectorId << 7 | this.collectorId >>> 25;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(28990);
         param1.writeInt(this.collectorId >>> 7 | this.collectorId << 25);
      }
   }
}

