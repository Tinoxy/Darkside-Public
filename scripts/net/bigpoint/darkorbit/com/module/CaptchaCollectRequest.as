package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class CaptchaCollectRequest implements IModule
   {
      private static var _instance:CaptchaCollectRequest;
      
      public var index:int = 0;
      
      public function CaptchaCollectRequest(param1:int = 0)
      {
         super();
         this.index = param1;
      }
      
      public static function get instance() : CaptchaCollectRequest
      {
         return _instance || (_instance = new CaptchaCollectRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 6087;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.index = param1.readInt();
         this.index = this.index >>> 4 | this.index << 28;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(6087);
         param1.writeInt(this.index << 4 | this.index >>> 28);
      }
   }
}

