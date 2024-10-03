Steps to make a .git hook for pre-commit testing:

**1**- Go to your .git/hook folder and check for a pre-commit file (.git/hooks/pre-commit).
    1.1- If it does not exist, create it.
    1.2- If it exists but has a .sample extension, remove it.

**2**- Delete its content and copy the following text to it:

#!/bin/sh
#
# An example hook script to verify what is about to be committed.
# Called by "git commit" with no arguments.  The hook should
# exit with non-zero status after issuing an appropriate message if
# it wants to stop the commit.
#
# To enable this hook, rename this file to "pre-commit".

if git rev-parse --verify HEAD >/dev/null 2>&1
then
	against=HEAD
else
	# Initial commit: diff against an empty tree object
	against=$(git hash-object -t tree /dev/null)
fi

# Adiciona a execução dos testes .NET
echo "Building project and running tests before commit..."

# Executar os testes .NET
dotnet test

# Verificar se os testes falharam
if [ $? -ne 0 ]; then
    echo "Build or Tests failed. Commit aborted."
    exit 1
fi

echo "Build sucess and all tests passed. Proceeding with commit."
exit 0

**3**- Make the new script executable with the command: chmod +x .git/hooks/pre-commit

Now if you try to commit tests that fail, the commit will be cancelled.