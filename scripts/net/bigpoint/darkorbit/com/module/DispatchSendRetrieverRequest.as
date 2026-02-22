package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DispatchSendRetrieverRequest implements IModule
   {
      private static var _instance:DispatchSendRetrieverRequest;
      
      public var dispatchId:int = 0;
      
      public function DispatchSendRetrieverRequest(param1:int = 0)
      {
         super();
         this.dispatchId = param1;
      }
      
      public static function get instance() : DispatchSendRetrieverRequest
      {
         return _instance || (_instance = new DispatchSendRetrieverRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 30664;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.dispatchId = param1.readInt();
         this.dispatchId = this.dispatchId << 1 | this.dispatchId >>> 31;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(30664);
         param1.writeInt(this.dispatchId >>> 1 | this.dispatchId << 31);
      }
   }
}

