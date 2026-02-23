package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MarkRangeCommand implements IModule
   {
      private static var _instance:MarkRangeCommand;
      
      public var uid:int = 0;
      
      public var color:int = 0;
      
      public var durationMillis:int = 0;
      
      public var range:int = 0;
      
      public function MarkRangeCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0)
      {
         super();
         this.uid = param1;
         this.color = param3;
         this.durationMillis = param4;
         this.range = param2;
      }
      
      public static function get instance() : MarkRangeCommand
      {
         return _instance || (_instance = new MarkRangeCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 5447;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.uid = param1.readInt();
         this.uid = this.uid << 15 | this.uid >>> 17;
         this.color = param1.readInt();
         this.color = this.color << 15 | this.color >>> 17;
         this.durationMillis = param1.readInt();
         this.durationMillis = this.durationMillis << 8 | this.durationMillis >>> 24;
         this.range = param1.readInt();
         this.range = this.range << 16 | this.range >>> 16;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(5447);
         param1.writeInt(this.uid >>> 15 | this.uid << 17);
         param1.writeInt(this.color >>> 15 | this.color << 17);
         param1.writeInt(this.durationMillis >>> 8 | this.durationMillis << 24);
         param1.writeInt(this.range >>> 16 | this.range << 16);
      }
   }
}

