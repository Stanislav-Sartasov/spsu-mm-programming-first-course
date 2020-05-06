	.file	"main.c"
	.text
	.section .rdata,"dr"
.LC0:
	.ascii "IEE 754x64: %d-\0"
.LC1:
	.ascii "%d\0"
	.text
	.p2align 4,,15
	.globl	showIEEEx64
	.def	showIEEEx64;	.scl	2;	.type	32;	.endef
	.seh_proc	showIEEEx64
showIEEEx64:
	pushq	%rbp
	.seh_pushreg	%rbp
	pushq	%rdi
	.seh_pushreg	%rdi
	pushq	%rsi
	.seh_pushreg	%rsi
	pushq	%rbx
	.seh_pushreg	%rbx
	subq	$312, %rsp
	.seh_stackalloc	312
	.seh_endprologue
	xorl	%eax, %eax
	movl	$53, %r10d
	movabsq	$18014398509481985, %rdx
	leaq	32(%rsp), %rbx
	movslq	%ecx, %rcx
	leaq	80(%rsp), %rsi
	subq	%rcx, %rdx
	movq	%rbx, %rdi
	movl	$5, %ecx
	rep stosq
	movl	$27, %ecx
	movq	%rsi, %rdi
	rep stosq
	leaq	212(%rsi), %rcx
	jmp	.L2
	.p2align 4,,10
.L5:
	movl	%r9d, %r10d
.L2:
	leal	-1(%r10), %r9d
	movq	%rdx, %r8
	shrq	$63, %r8
	leaq	(%rdx,%r8), %rax
	addq	%r8, %rdx
	andl	$1, %eax
	subq	%r8, %rax
	sarq	%rdx
	setne	%r8b
	cmpl	$-1, %r9d
	movl	%eax, (%rcx)
	setne	%al
	subq	$4, %rcx
	testb	%al, %r8b
	jne	.L5
	leaq	.LC0(%rip), %rcx
	movl	$181, %eax
	movq	%rbx, %rdi
	movq	$1, 44(%rsp)
	leaq	44(%rbx), %rbp
	subl	%r10d, %eax
	leaq	.LC1(%rip), %rbx
	movl	%eax, %edx
	andl	$1, %edx
	movl	%edx, 72(%rsp)
	movl	%eax, %edx
	sarl	%edx
	andl	$1, %edx
	movl	%edx, 68(%rsp)
	movl	%eax, %edx
	sarl	$2, %edx
	andl	$1, %edx
	movl	%edx, 64(%rsp)
	movl	%eax, %edx
	sarl	$3, %edx
	andl	$1, %edx
	movl	%edx, 60(%rsp)
	movl	%eax, %edx
	sarl	$5, %eax
	sarl	$4, %edx
	andl	$1, %eax
	andl	$1, %edx
	movl	%eax, 52(%rsp)
	movl	%edx, 56(%rsp)
	movl	80(%rsp), %edx
	call	printf
	.p2align 4,,10
.L3:
	movl	(%rdi), %edx
	movq	%rbx, %rcx
	addq	$4, %rdi
	call	printf
	cmpq	%rbp, %rdi
	jne	.L3
	leaq	8(%rsi), %rbx
	movl	$45, %ecx
	call	putchar
	leaq	216(%rsi), %rdi
	leaq	.LC1(%rip), %rsi
	.p2align 4,,10
.L4:
	movl	(%rbx), %edx
	movq	%rsi, %rcx
	addq	$4, %rbx
	call	printf
	cmpq	%rdi, %rbx
	jne	.L4
	movl	$10, %ecx
	addq	$312, %rsp
	popq	%rbx
	popq	%rsi
	popq	%rdi
	popq	%rbp
	jmp	putchar
	.seh_endproc
	.section .rdata,"dr"
.LC2:
	.ascii "inverted code+1: \0"
.LC3:
	.ascii "IEEE 754: 0-\0"
	.align 8
.LC4:
	.ascii "IEEE 754: 0-01111111-00000000000000000000000000000000\0"
	.text
	.p2align 4,,15
	.globl	showtwo
	.def	showtwo;	.scl	2;	.type	32;	.endef
	.seh_proc	showtwo
showtwo:
	pushq	%r13
	.seh_pushreg	%r13
	pushq	%r12
	.seh_pushreg	%r12
	pushq	%rbp
	.seh_pushreg	%rbp
	pushq	%rdi
	.seh_pushreg	%rdi
	pushq	%rsi
	.seh_pushreg	%rsi
	pushq	%rbx
	.seh_pushreg	%rbx
	subq	$200, %rsp
	.seh_stackalloc	200
	.seh_endprologue
	movabsq	$4294967297, %rdx
	leaq	64(%rsp), %rsi
	movslq	%ecx, %rax
	movl	$16, %ecx
	movq	$0, 32(%rsp)
	movq	%rax, %rbp
	subq	%rax, %rdx
	movq	%rsi, %rdi
	movq	$0, 40(%rsp)
	movq	$0, 48(%rsp)
	xorl	%eax, %eax
	rep stosq
	movl	$30, %ecx
	movq	$0, 56(%rsp)
	.p2align 4,,10
.L10:
	movq	%rdx, %r8
	shrq	$63, %r8
	leaq	(%rdx,%r8), %rax
	addq	%r8, %rdx
	andl	$1, %eax
	subq	%r8, %rax
	sarq	%rdx
	setne	%r8b
	cmpl	$-1, %ecx
	movl	%eax, 4(%rsi,%rcx,4)
	setne	%al
	subq	$1, %rcx
	testb	%al, %r8b
	jne	.L10
	leaq	.LC2(%rip), %rcx
	movq	%rsi, %rbx
	call	printf
	leaq	192(%rsp), %r12
	leaq	.LC1(%rip), %rdi
	.p2align 4,,10
.L11:
	movl	(%rbx), %edx
	movq	%rdi, %rcx
	addq	$4, %rbx
	call	printf
	cmpq	%r12, %rbx
	jne	.L11
	movl	$10, %ecx
	movq	%rsi, %rdi
	call	putchar
	xorl	%eax, %eax
	movl	$16, %ecx
	testl	%ebp, %ebp
	rep stosq
	je	.L12
	movl	$30, %edx
	.p2align 4,,10
.L13:
	movl	%ebp, %ecx
	shrl	$31, %ecx
	leal	0(%rbp,%rcx), %eax
	addl	%ecx, %ebp
	andl	$1, %eax
	subl	%ecx, %eax
	sarl	%ebp
	setne	%cl
	cmpl	$-1, %edx
	movl	%eax, 4(%rsi,%rdx,4)
	setne	%al
	subq	$1, %rdx
	testb	%al, %cl
	jne	.L13
.L12:
	xorl	%ebx, %ebx
	jmp	.L23
	.p2align 4,,10
.L14:
	cmpq	$31, %rbx
	je	.L39
	addq	$1, %rbx
.L23:
	cmpl	$1, (%rsi,%rbx,4)
	jne	.L14
	leal	1(%rbx), %r12d
	movl	$159, %edi
	leaq	.LC3(%rip), %rcx
	subl	%r12d, %edi
	call	printf
	movl	%edi, %eax
	andl	$1, %eax
	movl	%eax, 60(%rsp)
	movl	%edi, %eax
	sarl	%eax
	andl	$1, %eax
	movl	%eax, 56(%rsp)
	movl	%edi, %eax
	sarl	$2, %eax
	andl	$1, %eax
	movl	%eax, 52(%rsp)
	movl	%edi, %eax
	sarl	$3, %eax
	andl	$1, %eax
	movl	%eax, 48(%rsp)
	movl	%edi, %eax
	sarl	$4, %eax
	andl	$1, %eax
	movl	%eax, 44(%rsp)
	movl	%edi, %eax
	sarl	$5, %eax
	andl	$1, %eax
	movl	%eax, 40(%rsp)
	movl	%edi, %eax
	sarl	$7, %edi
	sarl	$6, %eax
	andl	$1, %eax
	testl	%edi, %edi
	movl	%eax, 36(%rsp)
	je	.L15
	movl	%edi, 32(%rsp)
.L15:
	leaq	32(%rsp), %rdi
	leaq	64(%rsp), %r13
	leaq	.LC1(%rip), %rbp
	.p2align 4,,10
.L16:
	movl	(%rdi), %edx
	movq	%rbp, %rcx
	addq	$4, %rdi
	call	printf
	cmpq	%r13, %rdi
	jne	.L16
	movl	$45, %ecx
	movl	$32, %ebp
	call	putchar
	subl	%r12d, %ebp
	cmpl	$32, %r12d
	je	.L20
	movslq	%r12d, %rax
	movl	$30, %r12d
	leaq	(%rsi,%rax,4), %rdi
	subl	%ebx, %r12d
	leaq	.LC1(%rip), %rbx
	addq	%r12, %rax
	leaq	4(%rsi,%rax,4), %rsi
	.p2align 4,,10
.L18:
	movl	(%rdi), %edx
	movq	%rbx, %rcx
	addq	$4, %rdi
	call	printf
	cmpq	%rsi, %rdi
	jne	.L18
	cmpl	$23, %ebp
	jg	.L22
	.p2align 4,,10
.L20:
	movl	$48, %ecx
	addl	$1, %ebp
	call	putchar
	cmpl	$24, %ebp
	jne	.L20
.L22:
	movl	$10, %ecx
	addq	$200, %rsp
	popq	%rbx
	popq	%rsi
	popq	%rdi
	popq	%rbp
	popq	%r12
	popq	%r13
	jmp	putchar
.L39:
	leaq	.LC4(%rip), %rcx
	call	printf
	jmp	.L22
	.seh_endproc
	.def	__main;	.scl	2;	.type	32;	.endef
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
	movl	$450, %ecx
	movq	%rdx, 192(%rsp)
	movq	$111, 120(%rsp)
	movq	$26723, 200(%rsp)
	call	showtwo
	movl	$450, %ecx
	call	showIEEEx64
	xorl	%eax, %eax
	addq	$272, %rsp
	popq	%rdi
	ret
	.seh_endproc
	.ident	"GCC: (x86_64-posix-seh-rev0, Built by MinGW-W64 project) 7.3.0"
	.def	printf;	.scl	2;	.type	32;	.endef
	.def	putchar;	.scl	2;	.type	32;	.endef
