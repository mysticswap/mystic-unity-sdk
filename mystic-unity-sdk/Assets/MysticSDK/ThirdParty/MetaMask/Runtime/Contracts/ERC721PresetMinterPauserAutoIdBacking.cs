//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : MetaMask Unity SDK ABI Code Generator
//   Input filename:  ERC721PresetMinterPauserAutoId.sol
//   Output filename: ERC721PresetMinterPauserAutoIdBacking.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------

#if UNITY_EDITOR || !ENABLE_MONO
using System;
using System.Numerics;
using System.Threading.Tasks;
using evm.net;
using evm.net.Models;

namespace MetaMask.Contracts
{
	public class ERC721PresetMinterPauserAutoIdBacking : Contract, ERC721PresetMinterPauserAutoId
	{
		public string Address
		{
			get => base.Address;
		}
		public ERC721PresetMinterPauserAutoIdBacking(IProvider provider, EvmAddress address, Type interfaceType) : base(provider, address, interfaceType)
		{
		}
		public Task<ERC721PresetMinterPauserAutoId> DeployNew(String name, String symbol, String baseTokenURI)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<ERC721PresetMinterPauserAutoId>) InvokeMethod(method, new object[] { name, symbol, baseTokenURI });
		}
		
		[EvmMethodInfo(Name = "DEFAULT_ADMIN_ROLE", View = true)]
		public Task<HexString> DEFAULT_ADMIN_ROLE()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<HexString>) InvokeMethod(method, new object[] {  });
		}
		
		[EvmMethodInfo(Name = "MINTER_ROLE", View = true)]
		public Task<HexString> MINTER_ROLE()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<HexString>) InvokeMethod(method, new object[] {  });
		}
		
		[EvmMethodInfo(Name = "PAUSER_ROLE", View = true)]
		public Task<HexString> PAUSER_ROLE()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<HexString>) InvokeMethod(method, new object[] {  });
		}

		public Task<ERC721> DeployNew(string name_, string symbol_)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<ERC721>) InvokeMethod(method, new object[] { name_, symbol_ });
		}

		[EvmMethodInfo(Name = "approve", View = false)]
		public Task<Transaction> Approve(EvmAddress to, BigInteger tokenId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { to, tokenId });
		}
		
		[EvmMethodInfo(Name = "balanceOf", View = true)]
		public Task<BigInteger> BalanceOf(EvmAddress owner)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<BigInteger>) InvokeMethod(method, new object[] { owner });
		}
		
		[EvmMethodInfo(Name = "burn", View = false)]
		public Task<Transaction> Burn(BigInteger tokenId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { tokenId });
		}
		
		[EvmMethodInfo(Name = "getApproved", View = true)]
		public Task<EvmAddress> GetApproved(BigInteger tokenId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<EvmAddress>) InvokeMethod(method, new object[] { tokenId });
		}
		
		[EvmMethodInfo(Name = "getRoleAdmin", View = true)]
		public Task<HexString> GetRoleAdmin(HexString role)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<HexString>) InvokeMethod(method, new object[] { role });
		}
		
		[EvmMethodInfo(Name = "getRoleMember", View = true)]
		public Task<EvmAddress> GetRoleMember(HexString role, BigInteger index)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<EvmAddress>) InvokeMethod(method, new object[] { role, index });
		}
		
		[EvmMethodInfo(Name = "getRoleMemberCount", View = true)]
		public Task<BigInteger> GetRoleMemberCount(HexString role)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<BigInteger>) InvokeMethod(method, new object[] { role });
		}
		
		[EvmMethodInfo(Name = "grantRole", View = false)]
		public Task<Transaction> GrantRole(HexString role, EvmAddress account)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { role, account });
		}
		
		[EvmMethodInfo(Name = "hasRole", View = true)]
		public Task<Boolean> HasRole(HexString role, EvmAddress account)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Boolean>) InvokeMethod(method, new object[] { role, account });
		}
		
		[EvmMethodInfo(Name = "isApprovedForAll", View = true)]
		public Task<Boolean> IsApprovedForAll(EvmAddress owner, [EvmParameterInfo(Type = "address", Name = "operator")] EvmAddress @operator)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Boolean>) InvokeMethod(method, new object[] { owner, @operator });
		}
		
		[EvmMethodInfo(Name = "mint", View = false)]
		public Task<Transaction> Mint(EvmAddress to)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { to });
		}
		
		[EvmMethodInfo(Name = "name", View = true)]
		public Task<String> Name()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<String>) InvokeMethod(method, new object[] {  });
		}
		
		[EvmMethodInfo(Name = "ownerOf", View = true)]
		public Task<EvmAddress> OwnerOf(BigInteger tokenId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<EvmAddress>) InvokeMethod(method, new object[] { tokenId });
		}
		
		[EvmMethodInfo(Name = "pause", View = false)]
		public Task<Transaction> Pause()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] {  });
		}
		
		[EvmMethodInfo(Name = "paused", View = true)]
		public Task<Boolean> Paused()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Boolean>) InvokeMethod(method, new object[] {  });
		}
		
		[EvmMethodInfo(Name = "renounceRole", View = false)]
		public Task<Transaction> RenounceRole(HexString role, EvmAddress account)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { role, account });
		}
		
		[EvmMethodInfo(Name = "revokeRole", View = false)]
		public Task<Transaction> RevokeRole(HexString role, EvmAddress account)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { role, account });
		}
		
		[EvmMethodInfo(Name = "safeTransferFrom", View = false)]
		public Task<Transaction> SafeTransferFrom(EvmAddress from, EvmAddress to, BigInteger tokenId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { from, to, tokenId });
		}
		
		[EvmMethodInfo(Name = "safeTransferFrom", View = false)]
		public Task<Transaction> SafeTransferFrom(EvmAddress from, EvmAddress to, BigInteger tokenId, Byte[] data)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { from, to, tokenId, data });
		}
		
		[EvmMethodInfo(Name = "setApprovalForAll", View = false)]
		public Task<Transaction> SetApprovalForAll([EvmParameterInfo(Type = "address", Name = "operator")] EvmAddress @operator, Boolean approved)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { @operator, approved });
		}
		
		[EvmMethodInfo(Name = "supportsInterface", View = true)]
		public Task<Boolean> SupportsInterface([EvmParameterInfo(Type = "bytes4", Name = "interfaceId")] Byte[] interfaceId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Boolean>) InvokeMethod(method, new object[] { interfaceId });
		}
		
		[EvmMethodInfo(Name = "symbol", View = true)]
		public Task<String> Symbol()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<String>) InvokeMethod(method, new object[] {  });
		}
		
		[EvmMethodInfo(Name = "tokenByIndex", View = true)]
		public Task<BigInteger> TokenByIndex(BigInteger index)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<BigInteger>) InvokeMethod(method, new object[] { index });
		}
		
		[EvmMethodInfo(Name = "tokenOfOwnerByIndex", View = true)]
		public Task<BigInteger> TokenOfOwnerByIndex(EvmAddress owner, BigInteger index)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<BigInteger>) InvokeMethod(method, new object[] { owner, index });
		}
		
		[EvmMethodInfo(Name = "tokenURI", View = true)]
		public Task<String> TokenURI(BigInteger tokenId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<String>) InvokeMethod(method, new object[] { tokenId });
		}
		
		[EvmMethodInfo(Name = "totalSupply", View = true)]
		public Task<BigInteger> TotalSupply()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<BigInteger>) InvokeMethod(method, new object[] {  });
		}
		
		[EvmMethodInfo(Name = "transferFrom", View = false)]
		public Task<Transaction> TransferFrom(EvmAddress from, EvmAddress to, BigInteger tokenId)
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] { from, to, tokenId });
		}
		
		[EvmMethodInfo(Name = "unpause", View = false)]
		public Task<Transaction> Unpause()
		{
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			return (Task<Transaction>) InvokeMethod(method, new object[] {  });
		}
		
	}
}
#endif
