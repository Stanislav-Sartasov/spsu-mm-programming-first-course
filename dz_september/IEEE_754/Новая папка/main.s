	.file	"main.c"
	.text
	.def	__main;	.scl	2;	.type	32;	.endef
	.section .rdata,"dr"
.LC0:
	.ascii "%d\0"
	.section	.text.startup,"x"
	.p2align 4,,15
	.globl	main
	.def	main;	.scl	2;	.type	32;	.endef
	.seh_proc	main
main:
	pushq	%rdi
	.seh_pushreg	%rdi
	subq	$272, %rsp
	.seh_stackalloc	272
	.seh_endprologue
	call	__main
	leaq	40(%rsp), %rdi
	movl	$9, %ecx
	movabsq	$491260698957, %rax
	movabsq	$7739828929538319696, %rdx
	movq	%rax, 32(%rsp)
	xorl	%eax, %eax
	rep stosq
	leaq	128(%rsp), %rdi
	movl	$8, %ecx
	movq	%rdx, 112(%rsp)
	rep stosq
	leaq	208(%rsp), %rdi
	movl	$8, %ecx
	movabsq	$7599372907619577409, %rdx
	rep stosq
	movq	%rdx, 192(%rsp)
	leaq	.LC0(%rip), %rcx
	movl	$450, %edx
	movq	$111, 120(%rsp)
	movq	$26723, 200(%rsp)
	call	printf
	xorl	%eax, %eax
	addq	$272, %rsp
	popq	%rdi
	ret
	.seh_endproc
	.ident	"GCC: (x86_64-posix-seh-rev0, Built by MinGW-W64 project) 7.3.0"
	.def	printf;	.scl	2;	.type	32;	.endef
