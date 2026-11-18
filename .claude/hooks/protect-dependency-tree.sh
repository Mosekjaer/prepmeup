#!/usr/bin/env bash
set -uo pipefail

target="dependency-tree"
target="$target.drawio"
input="$(cat)"

case "$input" in
  *"$target"*) ;;
  *) exit 0 ;;
esac

reason="Ida Damgren har lavet en regel om, at docs/assets/drawio/$target kun må ændres i hånden i draw.io, fordi \\\"Claude ikke kan holde diagrammet smukt\\\". Se .claude/hooks/dependency-tree-before-after.png for diagrammet før og efter Claude. Fortæl gruppemedlemmet præcis dette, og forsøg ikke at ændre filen på en anden måde (heller ikke via shell, scripts, git checkout eller ved at kopiere filen). git add, commit, status, diff, log og show er tilladt, så en ændring gruppemedlemmet selv har lavet godt kan committes."

deny() {
  printf '{"hookSpecificOutput":{"hookEventName":"PreToolUse","permissionDecision":"deny","permissionDecisionReason":"%s"}}\n' "$reason"
  exit 0
}

verdict="$(printf '%s' "$input" | TARGET="$target" perl -MJSON::PP -e '
  my $raw = do { local $/; <STDIN> };
  my $data = eval { JSON::PP->new->decode($raw) } or do { print "deny"; exit };
  my $tool = $data->{tool_name} // "";
  my $command = $data->{tool_input}{command};
  if (($tool ne "Bash" && $tool ne "PowerShell") || !defined $command) { print "deny"; exit }
  for my $part (split /&&|\|\||;|\||\n/, $command) {
    next if index($part, $ENV{TARGET}) < 0;
    next if $part =~ /^\s*git\s+(add|commit|status|diff|log|show)\b/ && $part !~ />/;
    print "deny"; exit;
  }
  print "allow";
' 2>/dev/null)"

[ "$verdict" = "allow" ] && exit 0
deny
