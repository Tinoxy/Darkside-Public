package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class UnstableModuleRecycleRequest implements IModule
   {
      private static var _instance:UnstableModuleRecycleRequest;
      
      public var itemId:int = 0;
      
      public var recycleReturns:int = 0;
      
      public function UnstableModuleRecycleRequest(param1:int = 0, param2:int = 0)
      {
         super();
         this.itemId = param1;
         this.recycleReturns = param2;
      }
      
      public static function get instance() : UnstableModuleRecycleRequest
      {
         return _instance || (_instance = new UnstableModuleRecycleRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return -29040;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.itemId = param1.readInt();
         this.itemId = this.itemId >>> 3 | this.itemId << 29;
         this.recycleReturns = param1.readInt();
         this.recycleReturns = this.recycleReturns << 6 | this.recycleReturns >>> 26;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-29040);
         param1.writeInt(this.itemId << 3 | this.itemId >>> 29);
         param1.writeInt(this.recycleReturns >>> 6 | this.recycleReturns << 26);
      }
   }
}

