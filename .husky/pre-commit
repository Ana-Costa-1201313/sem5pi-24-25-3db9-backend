#!/bin/sh
. "$(dirname "$0")/_/husky.sh"

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