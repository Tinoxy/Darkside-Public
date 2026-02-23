package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AgatusEventTimeInfoCommand implements IModule
   {
      private static var _instance:AgatusEventTimeInfoCommand;
      
      public var timeToEndSpawn:Number = 0;
      
      public var timeToNextSpawn:Number = 0;
      
      public function AgatusEventTimeInfoCommand(param1:Number = 0, param2:Number = 0)
      {
         super();
         this.timeToEndSpawn = param1;
         this.timeToNextSpawn = param2;
      }
      
      public static function get instance() : AgatusEventTimeInfoCommand
      {
         return _instance || (_instance = new AgatusEventTimeInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 6832;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.timeToEndSpawn = param1.readDouble();
         this.timeToNextSpawn = param1.readDouble();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(6832);
         param1.writeDouble(this.timeToEndSpawn);
         param1.writeDouble(this.timeToNextSpawn);
      }
   }
}

