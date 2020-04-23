// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
	"Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores",
	Justification = "Test methods require underscores for readability.",
	Scope = "Sakamoto")]
[assembly: SuppressMessage(
	"Style", "IDE0060:사용하지 않는 매개 변수를 제거하세요.",
	Justification = "<보류 중>",
	Scope = "Sakamoto")]
