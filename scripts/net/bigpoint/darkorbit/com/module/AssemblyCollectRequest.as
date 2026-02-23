package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AssemblyCollectRequest implements IModule
   {
      private static var _instance:AssemblyCollectRequest;
      
      public var targetItemId:Number = 0;
      
      public var recipeLootId:String = "";
      
      public function AssemblyCollectRequest(param1:Number = 0, param2:String = "")
      {
         super();
         this.targetItemId = param1;
         this.recipeLootId = param2;
      }
      
      public static function get instance() : AssemblyCollectRequest
      {
         return _instance || (_instance = new AssemblyCollectRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 31383;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.targetItemId = param1.readDouble();
         this.recipeLootId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(31383);
         param1.writeDouble(this.targetItemId);
         param1.writeUTF(this.recipeLootId);
      }
   }
}

