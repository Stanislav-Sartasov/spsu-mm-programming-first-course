	.file	"main.c"
	.text
	.def	__main;	.scl	2;	.type	32;	.endef
	.globl	main
	.def	main;	.scl	2;	.type	32;	.endef
	.seh_proc	main
main:
	pushq	%rbp
	.seh_pushreg	%rbp
	pushq	%rdi
	.seh_pushreg	%rdi
	pushq	%rbx
	.seh_pushreg	%rbx
	subq	$288, %rsp
	.seh_stackalloc	288
	leaq	128(%rsp), %rbp
	.seh_setframe	%rbp, 128
	.seh_endprologue
	call	__main
	movabsq	$491260698957, %rax
	movq	%rax, 64(%rbp)
	leaq	72(%rbp), %rdx
	movl	$0, %eax
	movl	$9, %ecx
	movq	%rdx, %rdi
	rep stosq
	movabsq	$7739828929538319696, %rax
	movq	%rax, -16(%rbp)
	movq	$111, -8(%rbp)
	movq	%rbp, %rdx
	movl	$0, %eax
	movl	$8, %ecx
	movq	%rdx, %rdi
	rep stosq
	movabsq	$7599372907619577409, %rax
	movq	%rax, -96(%rbp)
	movq	$26723, -88(%rbp)
	leaq	-80(%rbp), %rdx
	movl	$0, %eax
	movl	$8, %ecx
	movq	%rdx, %rdi
	rep stosq
	leaq	64(%rbp), %rax
	movq	%rax, %rcx
	call	strlen
	movl	%eax, %ebx
	leaq	-16(%rbp), %rax
	movq	%rax, %rcx
	call	strlen
	imull	%eax, %ebx
	leaq	-96(%rbp), %rax
	movq	%rax, %rcx
	call	strlen
	imull	%ebx, %eax
	movl	%eax, 156(%rbp)
	movl	156(%rbp), %eax
	movl	%eax, %ecx
	call	showtwo
	movl	156(%rbp), %eax
	movl	%eax, %ecx
	call	showIEEEx64
	movl	$0, %eax
	addq	$288, %rsp
	popq	%rbx
	popq	%rdi
	popq	%rbp
	ret
	.seh_endproc
	.section .rdata,"dr"
.LC0:
	.ascii "IEE 754x64: %d-\0"
.LC1:
	.ascii "%d\0"
	.text
	.globl	showIEEEx64
	.def	showIEEEx64;	.scl	2;	.type	32;	.endef
	.seh_proc	showIEEEx64
showIEEEx64:
	pushq	%rbp
	.seh_pushreg	%rbp
	pushq	%rbx
	.seh_pushreg	%rbx
	subq	$328, %rsp
	.seh_stackalloc	328
	leaq	128(%rsp), %rbp
	.seh_setframe	%rbp, 128
	.seh_endprologue
	movl	%ecx, 224(%rbp)
	movl	224(%rbp), %eax
	cltq
	movabsq	$18014398509481985, %rdx
	subq	%rax, %rdx
	movq	%rdx, %rax
	movq	%rax, 184(%rbp)
	movl	$53, 180(%rbp)
	movl	$0, 176(%rbp)
	movl	$10, 172(%rbp)
	movl	$0, %ebx
	jmp	.L4
.L5:
	movslq	%ebx, %rax
	movl	$0, -96(%rbp,%rax,4)
	addl	$1, %ebx
.L4:
	cmpl	$10, %ebx
	jle	.L5
	movl	$0, %ebx
	jmp	.L6
.L7:
	movslq	%ebx, %rax
	movl	$0, -48(%rbp,%rax,4)
	addl	$1, %ebx
.L6:
	cmpl	$53, %ebx
	jle	.L7
	jmp	.L8
.L10:
	movq	184(%rbp), %rax
	cqto
	shrq	$63, %rdx
	addq	%rdx, %rax
	andl	$1, %eax
	subq	%rdx, %rax
	movq	%rax, %rcx
	movl	180(%rbp), %eax
	leal	-1(%rax), %edx
	movl	%edx, 180(%rbp)
	movl	%ecx, %edx
	cltq
	movl	%edx, -48(%rbp,%rax,4)
	movq	184(%rbp), %rax
	movq	%rax, %rdx
	shrq	$63, %rdx
	addq	%rdx, %rax
	sarq	%rax
	movq	%rax, 184(%rbp)
.L8:
	cmpq	$0, 184(%rbp)
	je	.L9
	cmpl	$-1, 180(%rbp)
	jne	.L10
.L9:
	addl	$1, 180(%rbp)
	movl	$181, %eax
	subl	180(%rbp), %eax
	movl	%eax, 176(%rbp)
	jmp	.L11
.L13:
	movl	172(%rbp), %eax
	leal	-1(%rax), %edx
	movl	%edx, 172(%rbp)
	movl	176(%rbp), %edx
	movl	%edx, %ecx
	sarl	$31, %ecx
	shrl	$31, %ecx
	addl	%ecx, %edx
	andl	$1, %edx
	subl	%ecx, %edx
	cltq
	movl	%edx, -96(%rbp,%rax,4)
	movl	176(%rbp), %eax
	movl	%eax, %edx
	shrl	$31, %edx
	addl	%edx, %eax
	sarl	%eax
	movl	%eax, 176(%rbp)
.L11:
	cmpl	$0, 176(%rbp)
	je	.L12
	cmpl	$-1, 172(%rbp)
	jne	.L13
.L12:
	movl	-48(%rbp), %eax
	movl	%eax, %edx
	leaq	.LC0(%rip), %rcx
	call	printf
	movl	$0, %ebx
	jmp	.L14
.L15:
	movslq	%ebx, %rax
	movl	-96(%rbp,%rax,4), %eax
	movl	%eax, %edx
	leaq	.LC1(%rip), %rcx
	call	printf
	addl	$1, %ebx
.L14:
	cmpl	$10, %ebx
	jle	.L15
	movl	$45, %ecx
	call	putchar
	movl	$2, %ebx
	jmp	.L16
.L17:
	movslq	%ebx, %rax
	movl	-48(%rbp,%rax,4), %eax
	movl	%eax, %edx
	leaq	.LC1(%rip), %rcx
	call	printf
	addl	$1, %ebx
.L16:
	cmpl	$53, %ebx
	jle	.L17
	movl	$10, %ecx
	call	putchar
	nop
	addq	$328, %rsp
	popq	%rbx
	popq	%rbp
	ret
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
	.globl	showtwo
	.def	showtwo;	.scl	2;	.type	32;	.endef
	.seh_proc	showtwo
showtwo:
	pushq	%rbp
	.seh_pushreg	%rbp
	pushq	%rbx
	.seh_pushreg	%rbx
	subq	$232, %rsp
	.seh_stackalloc	232
	leaq	128(%rsp), %rbp
	.seh_setframe	%rbp, 128
	.seh_endprologue
	movl	%ecx, 128(%rbp)
	movl	128(%rbp), %eax
	cltq
	movabsq	$4294967297, %rdx
	subq	%rax, %rdx
	movq	%rdx, %rax
	movq	%rax, 88(%rbp)
	movl	$31, 84(%rbp)
	movl	$0, 80(%rbp)
	movl	$0, %ebx
	jmp	.L19
.L20:
	movslq	%ebx, %rax
	movl	$0, -96(%rbp,%rax,4)
	addl	$1, %ebx
.L19:
	cmpl	$7, %ebx
	jle	.L20
	movl	$0, %ebx
	jmp	.L21
.L22:
	movslq	%ebx, %rax
	movl	$0, -64(%rbp,%rax,4)
	addl	$1, %ebx
.L21:
	cmpl	$31, %ebx
	jle	.L22
	jmp	.L23
.L25:
	movq	88(%rbp), %rax
	cqto
	shrq	$63, %rdx
	addq	%rdx, %rax
	andl	$1, %eax
	subq	%rdx, %rax
	movq	%rax, %rcx
	movl	84(%rbp), %eax
	leal	-1(%rax), %edx
	movl	%edx, 84(%rbp)
	movl	%ecx, %edx
	cltq
	movl	%edx, -64(%rbp,%rax,4)
	movq	88(%rbp), %rax
	movq	%rax, %rdx
	shrq	$63, %rdx
	addq	%rdx, %rax
	sarq	%rax
	movq	%rax, 88(%rbp)
.L23:
	cmpq	$0, 88(%rbp)
	je	.L24
	cmpl	$-1, 84(%rbp)
	jne	.L25
.L24:
	addl	$1, 84(%rbp)
	leaq	.LC2(%rip), %rcx
	call	printf
	movl	$0, %ebx
	jmp	.L26
.L27:
	movslq	%ebx, %rax
	movl	-64(%rbp,%rax,4), %eax
	movl	%eax, %edx
	leaq	.LC1(%rip), %rcx
	call	printf
	addl	$1, %ebx
.L26:
	cmpl	$31, %ebx
	jle	.L27
	movl	$10, %ecx
	call	putchar
	movl	$31, 84(%rbp)
	movl	128(%rbp), %eax
	cltq
	movq	%rax, 88(%rbp)
	movl	$0, %ebx
	jmp	.L28
.L29:
	movslq	%ebx, %rax
	movl	$0, -64(%rbp,%rax,4)
	addl	$1, %ebx
.L28:
	cmpl	$31, %ebx
	jle	.L29
	jmp	.L30
.L32:
	movl	84(%rbp), %eax
	leal	-1(%rax), %edx
	movl	%edx, 84(%rbp)
	movl	128(%rbp), %edx
	movl	%edx, %ecx
	sarl	$31, %ecx
	shrl	$31, %ecx
	addl	%ecx, %edx
	andl	$1, %edx
	subl	%ecx, %edx
	cltq
	movl	%edx, -64(%rbp,%rax,4)
	movl	128(%rbp), %eax
	movl	%eax, %edx
	shrl	$31, %edx
	addl	%edx, %eax
	sarl	%eax
	movl	%eax, 128(%rbp)
.L30:
	cmpl	$0, 128(%rbp)
	je	.L31
	cmpl	$-1, 84(%rbp)
	jne	.L32
.L31:
	movl	$0, %ebx
	jmp	.L33
.L46:
	movslq	%ebx, %rax
	movl	-64(%rbp,%rax,4), %eax
	cmpl	$1, %eax
	jne	.L34
	leal	1(%rbx), %eax
	movl	%eax, 84(%rbp)
	movl	$159, %eax
	subl	84(%rbp), %eax
	movl	%eax, 80(%rbp)
	leaq	.LC3(%rip), %rcx
	call	printf
	movl	$7, 76(%rbp)
	jmp	.L35
.L37:
	movl	76(%rbp), %eax
	leal	-1(%rax), %edx
	movl	%edx, 76(%rbp)
	movl	80(%rbp), %edx
	movl	%edx, %ecx
	sarl	$31, %ecx
	shrl	$31, %ecx
	addl	%ecx, %edx
	andl	$1, %edx
	subl	%ecx, %edx
	cltq
	movl	%edx, -96(%rbp,%rax,4)
	movl	80(%rbp), %eax
	movl	%eax, %edx
	shrl	$31, %edx
	addl	%edx, %eax
	sarl	%eax
	movl	%eax, 80(%rbp)
.L35:
	cmpl	$0, 80(%rbp)
	je	.L36
	cmpl	$-1, 76(%rbp)
	jne	.L37
.L36:
	movl	$0, 72(%rbp)
	jmp	.L38
.L39:
	movl	72(%rbp), %eax
	cltq
	movl	-96(%rbp,%rax,4), %eax
	movl	%eax, %edx
	leaq	.LC1(%rip), %rcx
	call	printf
	addl	$1, 72(%rbp)
.L38:
	cmpl	$7, 72(%rbp)
	jle	.L39
	movl	$45, %ecx
	call	putchar
	movl	84(%rbp), %ebx
	jmp	.L40
.L41:
	movslq	%ebx, %rax
	movl	-64(%rbp,%rax,4), %eax
	movl	%eax, %edx
	leaq	.LC1(%rip), %rcx
	call	printf
	addl	$1, %ebx
.L40:
	cmpl	$31, %ebx
	jle	.L41
	movl	$32, %eax
	subl	84(%rbp), %eax
	movl	%eax, %ebx
	jmp	.L42
.L43:
	movl	$48, %ecx
	call	putchar
	addl	$1, %ebx
.L42:
	cmpl	$23, %ebx
	jle	.L43
	jmp	.L44
.L34:
	cmpl	$31, %ebx
	jne	.L45
	leaq	.LC4(%rip), %rcx
	call	printf
.L45:
	addl	$1, %ebx
.L33:
	cmpl	$31, %ebx
	jle	.L46
.L44:
	movl	$10, %ecx
	call	putchar
	nop
	addq	$232, %rsp
	popq	%rbx
	popq	%rbp
	ret
	.seh_endproc
	.ident	"GCC: (x86_64-posix-seh-rev0, Built by MinGW-W64 project) 7.3.0"
	.def	strlen;	.scl	2;	.type	32;	.endef
	.def	printf;	.scl	2;	.type	32;	.endef
	.def	putchar;	.scl	2;	.type	32;	.endef
