package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PingPositionRequest implements IModule
   {
      private static var _instance:PingPositionRequest;
      
      public var x:int = 0;
      
      public var y:int = 0;
      
      public function PingPositionRequest(param1:int = 0, param2:int = 0)
      {
         super();
         this.x = param1;
         this.y = param2;
      }
      
      public static function get instance() : PingPositionRequest
      {
         return _instance || (_instance = new PingPositionRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 28901;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.x = param1.readInt();
         this.x = this.x << 4 | this.x >>> 28;
         this.y = param1.readInt();
         this.y = this.y << 4 | this.y >>> 28;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(28901);
         param1.writeInt(this.x >>> 4 | this.x << 28);
         param1.writeInt(this.y >>> 4 | this.y << 28);
      }
   }
}

