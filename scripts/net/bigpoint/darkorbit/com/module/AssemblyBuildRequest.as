package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AssemblyBuildRequest implements IModule
   {
      private static var _instance:AssemblyBuildRequest;
      
      public var amount:Number = 0;
      
      public var recipeLootId:String = "";
      
      public var targetItemId:Number = 0;
      
      public function AssemblyBuildRequest(param1:String = "", param2:Number = 0, param3:Number = 0)
      {
         super();
         this.amount = param2;
         this.recipeLootId = param1;
         this.targetItemId = param3;
      }
      
      public static function get instance() : AssemblyBuildRequest
      {
         return _instance || (_instance = new AssemblyBuildRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 4345;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 18;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.amount = param1.readDouble();
         this.recipeLootId = param1.readUTF();
         this.targetItemId = param1.readDouble();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(4345);
         param1.writeDouble(this.amount);
         param1.writeUTF(this.recipeLootId);
         param1.writeDouble(this.targetItemId);
      }
   }
}

