package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DispatchCollectRetrieverRequest implements IModule
   {
      private static var _instance:DispatchCollectRetrieverRequest;
      
      public var slotId:int = 0;
      
      public function DispatchCollectRetrieverRequest(param1:int = 0)
      {
         super();
         this.slotId = param1;
      }
      
      public static function get instance() : DispatchCollectRetrieverRequest
      {
         return _instance || (_instance = new DispatchCollectRetrieverRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return -63;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 2;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.slotId = param1.readShort();
         this.slotId = 65535 & ((65535 & this.slotId) >>> 4 | (65535 & this.slotId) << 12);
         this.slotId = this.slotId > 32767 ? int(this.slotId - 65536) : this.slotId;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-63);
         param1.writeShort(65535 & ((65535 & this.slotId) << 4 | (65535 & this.slotId) >>> 12));
      }
   }
}

