package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PayloadEscortScoreCommand implements IModule
   {
      private static var _instance:PayloadEscortScoreCommand;
      
      public var totalScore:int = 0;
      
      public var runScore:int = 0;
      
      public function PayloadEscortScoreCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.totalScore = param2;
         this.runScore = param1;
      }
      
      public static function get instance() : PayloadEscortScoreCommand
      {
         return _instance || (_instance = new PayloadEscortScoreCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 25150;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.totalScore = param1.readInt();
         this.totalScore = this.totalScore >>> 14 | this.totalScore << 18;
         this.runScore = param1.readInt();
         this.runScore = this.runScore >>> 6 | this.runScore << 26;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(25150);
         param1.writeInt(this.totalScore << 14 | this.totalScore >>> 18);
         param1.writeInt(this.runScore << 6 | this.runScore >>> 26);
      }
   }
}

