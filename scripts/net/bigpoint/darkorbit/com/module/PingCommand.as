package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PingCommand implements IModule
   {
      private static var _instance:PingCommand;
      
      public var y:int = 0;
      
      public var x:int = 0;
      
      public function PingCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.y = param2;
         this.x = param1;
      }
      
      public static function get instance() : PingCommand
      {
         return _instance || (_instance = new PingCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 20628;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.y = param1.readInt();
         this.y = this.y >>> 5 | this.y << 27;
         this.x = param1.readInt();
         this.x = this.x >>> 13 | this.x << 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(20628);
         param1.writeInt(this.y << 5 | this.y >>> 27);
         param1.writeInt(this.x << 13 | this.x >>> 19);
      }
   }
}

