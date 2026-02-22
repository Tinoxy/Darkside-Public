package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class InvitationRejectRequest implements IModule
   {
      private static var _instance:InvitationRejectRequest;
      
      public var inviterId:int = 0;
      
      public function InvitationRejectRequest(param1:int = 0)
      {
         super();
         this.inviterId = param1;
      }
      
      public static function get instance() : InvitationRejectRequest
      {
         return _instance || (_instance = new InvitationRejectRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 27456;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.inviterId = param1.readInt();
         this.inviterId = this.inviterId >>> 1 | this.inviterId << 31;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(27456);
         param1.writeInt(this.inviterId << 1 | this.inviterId >>> 31);
      }
   }
}

