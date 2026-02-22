package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class CCNumericRangeModule extends CCAbstractRangeModule
   {
      private static var _instance:CCNumericRangeModule;
      
      public var min:int = 0;
      
      public var max:int = 0;
      
      public function CCNumericRangeModule(param1:int = 0, param2:int = 0)
      {
         super();
         this.min = param1;
         this.max = param2;
      }
      
      public static function get instance() : CCNumericRangeModule
      {
         return _instance || (_instance = new CCNumericRangeModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 27934;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.min = param1.readInt();
         this.min = this.min >>> 9 | this.min << 23;
         this.max = param1.readInt();
         this.max = this.max >>> 10 | this.max << 22;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(27934);
         super.write(param1);
         param1.writeInt(this.min << 9 | this.min >>> 23);
         param1.writeInt(this.max << 10 | this.max >>> 22);
      }
   }
}

