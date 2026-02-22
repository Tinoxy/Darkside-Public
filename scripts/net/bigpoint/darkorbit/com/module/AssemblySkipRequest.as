package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AssemblySkipRequest implements IModule
   {
      private static var _instance:AssemblySkipRequest;
      
      public var targetItemId:Number = 0;
      
      public var recipeLootId:String = "";
      
      public function AssemblySkipRequest(param1:String = "", param2:Number = 0)
      {
         super();
         this.targetItemId = param2;
         this.recipeLootId = param1;
      }
      
      public static function get instance() : AssemblySkipRequest
      {
         return _instance || (_instance = new AssemblySkipRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 14647;
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
         param1.writeShort(14647);
         param1.writeDouble(this.targetItemId);
         param1.writeUTF(this.recipeLootId);
      }
   }
}

